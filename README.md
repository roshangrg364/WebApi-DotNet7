# WebApiDemo-DotNet7
WebApi with rest pattern, basic jwt authentication and a MvcProject to consume the api using a simple Villa Crud operation

#what the project consist of
- It has two project VillaAPi and VillaWebApp
- VillApi is the WebApi Project that consist of api's following Rest Pattern for the basic Crud of Villa.
- Basic Jwt Authentication with Asp Dot net core Identity is used in villaApi
- versioning have been maintained.
- VillaWebApp is used to consume the api's on VillaAPi App
- Villa Api Service Code have been maintained in coreModule library and VillaWebApp Service code have been maintained in Villa_MVC_CoreModule Library

#How To Run the prpject
- since the VillawebApp consumes api's from VillaApi both project should run at same time.
- so setup multiple startup prpject from ConfigureStartupOptions in solution explorer
- setup the connection string in appsetting.json in villaApi project
- You can change the port of project on launchsetting.json and set the baseUrlEndpoint of villaApi on URLDatas class in Villa_MVC_Core_module
- once the setup is ready -run the project.. on startup database is automatically generated or you can run ef core migration .

