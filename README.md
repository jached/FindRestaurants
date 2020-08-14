# FindRestaurants
Find nearby restaurants (not serious)

Function apps in Azure triggered by http requests

User information is stored in Azure Cosmos DB (just wanted to try it)

## GetRestaurants function app
Gets an Area object that contains radius set by user and coordinates of users position. This uses Google Places API to get nearby restaurants.

## MyAccount function app
Contains two functions, CreateAccount and Login.

- Login takes a User object that contains username and password. The app connects to Azure Cosmos DB and checks that username and password is correct. 
Then a jwt token is created with username and id as claims. The response from the function contains the users information and a header with the jwt token.

- CreateAccount takes a User object that contains username and password. The app connects to Azure Cosmos DB and adds the user with a GUID as id. A jwt token is created as above
and the response is the same as above.

## MyRestaurants function app
Contains three functions, GetMyRestaurants, SaveRestaurant and RemoveRestaurant.

- GetMyRestaurants takes the jwt token as a header and gets username and id from that. The app connects to Azure Cosmos DB and gets the users saved restaurants. 
The rsponse contains a list of restaurants.

- SaveRestaurant takes the jwt token as above and a Restaurant object. The app connects to Azure Cosmos DB and adds the restaurant to the users saved restaurants.

- RemoveRestaurant takes the jwt token as above and a Restaurant object. The app connects to Azure Cosmos DB and removes the restaurant to the users saved restaurants.

## Startup
All function apps has a startup class which gets DbSettings, JWTSettings and Key Vault url from Azure App Configuration using user assigned managed identity. 
The API key to Google and the authentication key for Cosmos DB (Cosmos DB doesn't support managed identity at this time) are fetched from Azure Key Vault using the same 
user assigned managed identity.
