# Skycave.MessageService

# Configuration

The service uses the feature toggles and configuration files to manage whether to use the FakeStorageService or not.
The configuration file for Test environment has been configured to use FakeStorage and Production has been set to use real storage.
If the service wont run when specifying Production environment run with real storage, it might be because it has not been implemented yet.

# To build and run do as follows:

**[BUILD]:** `docker build -t SkycaveMessageService .`

**NOTE:** Make sure your cmd is in the root of this solution where the dockerfile is placed

**[RUN]:** `docker run -d -p 10001:10001 -e DOTNET_ENVIRONMENT=Test --rm --name REPLACE_ME SkycaveMessageService`

**NOTE:** The following run command will prompt the service to run in Test.
To change this, and thereby change which configuration to use, change the argument '-e DOTNET_ENVIRONMENT=Test'.
