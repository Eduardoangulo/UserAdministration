# UserAdministration

# Solution

First of all, I would like to start saying that this solution consists on a stable MVP of the desired API from my perspective. There are lot of things that should be improved in production environment but it depends on more than 1 scenarios that we are supposed to handle. All the specs that I would like to update are going to be shared during my explanation.

From a quick view of the solution, the first perspective should be how I will handle the architecture of the solution:

<img width="468" alt="image" src="https://user-images.githubusercontent.com/25852192/166312583-3975facf-8efc-434e-8c83-64f499592031.png">


At the beginning, I decided to have an API developed on C# language using .NET Core 5 framework. I made that decision because my experience using it, and its testing was made using NUnit. Then, I decided to allocate the API on 2 different Ubuntu servers, using EC2 Amazon Web Services but using Docker because the possibility to work with that containers and images and configure them more quickly. I think that Amazon provides a very easy-handle way to work with servers.
 
 <img width="468" alt="image" src="https://user-images.githubusercontent.com/25852192/166312619-759d07c9-317e-4396-9a04-053f795fc762.png">


I did use MySql database because for me is smooth than work with on-memory databases. Then I decided to allocate my database with RDS service from AWS.
I did try to use also Azure databases because its integration but I did prefer to use all the possible staff from AWS.

Then, the challenging part for me was the configuration of the Balancing and the Scale for the servers. I decided to allocate Nginx on an EC2 instance using Ubuntu OS too. So, this server will work as a Load Balancer between both servers. I did not choose a Load Balancer from Amazon because its direct benefit to the solution is not clear, I think that the use of Docker helps me to try and understand containerization and its use at a different level. Then for auto-scale I did choose to use Auto Scaling from AWS because its quickest way to launch a different instance from EC2 and its availability. The thing here is that sometimes my instance is landing a bad gateway and I think it’s because Docker running stops during a creation of a new instance. It’s kind of weird but being honest I think is the part of the whole integration that I would like to know a bit more I did not have much experience on auto scaling servers.

# Extra
I decided to implement a Hash class to protect passwords using. To actually protect the password, I did use the implementation of the PBKDF2 (RFC 2898) algorithm supplied in the .NET Core runtime. It’s a battle tested algorithm that takes a password and runs it through a hash algorithm (I am using SHA256*) a set number of times which at the end produces a large blob of binary to use as a “key”. This process is referred to as key derivation. 
 
# Repository Pattern

I did choose to use Repository pattern and Dependency injection as a part of my whole development.

Repository pattern is a design pattern that mediates data from and to the Domain and Data Access Layers ( like Entity Framework Core / Dapper). Repositories are classes that hide the logics required to store or retreive data.

Benefits: 
•	Reduces Duplicate Queries
•	De-couples the application from the Data Access Layer

<img width="468" alt="image" src="https://user-images.githubusercontent.com/25852192/166312652-990a6576-227c-4769-8684-890050e754e5.png">
