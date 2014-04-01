PhoneGame
=========
Use this url to test with, make sure your local server is running as http://localhost:55961:
http://localhost:55961/home/GoogleOAuth


You need the file PhoneGameWebApi\HiddenSettings.config to override your OAuth userids and OAuth client secrets for OAuth to work.  You can override the fake appSettings keys in the Web.config- look at the example below.

For Google OAuth (PhoneGameWebApi\HiddenSettings.config)
```
<?xml version="1.0"?>
<appSettings>
  <add key="GoogleAppID" value="id" />
  <add key="GoogleAppSecret" value="secret" />
</appSettings>
```
