PhoneGame
=========
Use this url to test OAuth with, make sure your port is set to 55961:
<a href="http://localhost:55961/home/GoogleOAuth" target="_blank">http://localhost:55961/home/GoogleOAuth</a>


You need the file PhoneGameWebApi\HiddenSettings.config to override your OAuth userids and OAuth client secrets for OAuth to work.  You can override the fake appSettings keys in the Web.config- look at the example below.

For Google OAuth (PhoneGameWebApi\HiddenSettings.config)
```
<?xml version="1.0"?>
<appSettings>
  <add key="GoogleAppID" value="id" />
  <add key="GoogleAppSecret" value="secret" />
</appSettings>
```
