PhoneGame
=========
To test the apps front end, go to /Cordova/www/index.html

You need the file PhoneGameWebApi\HiddenSettings.config to override your OAuth userids and OAuth client secrets for OAuth to work.  You can override the fake appSettings keys in the Web.config- look at the example below.
For Google OAuth (PhoneGameWebApi\HiddenSettings.config)
```
<?xml version="1.0"?>
<appSettings>
  <add key="GoogleAppID" value="id" />
  <add key="GoogleAppSecret" value="secret" />
</appSettings>
```
