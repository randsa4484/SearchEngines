# Search Engines Technical Test

To run this code you will need a API Keys for Bing and Google. These have NOT been committed to SourceControl as they are not to be made public.

Visual Studio project secrets is covered here:

https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows

The keys can be generated via the methods outlined here, or the keys used in development can be obtained from Andrew Rands

https://stackoverflow.com/questions/4082966/what-are-the-alternatives-now-that-the-google-web-search-api-has-been-deprecated
https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/web-search-sdk-quickstart

Once the keys are available, they can be put into secrets.json in 

C:\Users\<your user name>\AppData\Roaming\Microsoft\UserSecrets\83eb9069-e345-4445-853e-c437d913f31f

Where secrets.json is this format:

{
  "Google": {
    "apiKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
    "cx": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
  },
  "Bing": {
    "apiKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
  } 
}

Alternatively, these sections can be pasted into Bds.TechTest\appSettings.Development.json

The solution consists of a WebAPI project, Bds.TechTest and an MVC Website, Bds.TechTest.Web. These two projects should both be startup projects. 

There is an additional Class library, SearchAggregator. The main reason for using this is to demonstrate using builder code in the Bds.TechTest\Startup.cs to support strongly typed configuration of library classes that use DI.

All three projects have accompanying unit test projects.

The WebAPI project uses Swagger to produce a UI that enables WebAPI consumers to understand the endpoints and likely responses.
