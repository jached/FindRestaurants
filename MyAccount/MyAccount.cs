using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurants.Shared.Models;
using Restaurants.Shared;
using MyAccount.Services;
using Azure;
using Azure.Core;
using System.Text;

namespace MyAccount
{
    public class MyAccount
    {
        public JWTHandler JWTHandler { get; }
        public IAccountService AccountService { get; }

        public MyAccount(JWTHandler jwtHandler, IAccountService accountService)
        {
            JWTHandler = jwtHandler;
            AccountService = accountService;
        }

        [FunctionName("Login")]
        public async Task<HttpResponse> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            string jwt;
            try
            {
                User userFromRespons = await AccountService.Login(user);
                try
                {
                    jwt = JWTHandler.GetJWT(userFromRespons);
                    return await GenerateResponse(jwt, userFromRespons, req);
                }
                catch (Exception e)
                {
                    log.LogError("Failed to generate token", e);
                    throw;
                }
            }
            catch (Exception)
            {
                var context = req.HttpContext;
                var response = context.Response;
                response.StatusCode = 401;
                await response.WriteAsync("Failed to authorize");
                return response;
            }
        }

        [FunctionName("CreateAccount")]
        public async Task<HttpResponse> CreateAccount(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            try
            {
                user = await AccountService.CreateAccount(user);
                string jwt;
                try
                {
                    jwt = JWTHandler.GetJWT(user);
                }
                catch (Exception e)
                {
                    log.LogError("Failed to generate token", e);
                    throw;
                }

                return await GenerateResponse(jwt, user, req);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<HttpResponse> GenerateResponse(string jwt, User user, HttpRequest req)
        {
            var json = JsonConvert.SerializeObject(user);
            var context = req.HttpContext;
            var response = context.Response;
            response.Headers.Add("JWTToken", jwt);
            await response.WriteAsync(json);
            return response;
        }
    }
}
