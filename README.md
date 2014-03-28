PhoneGame
=========
Use a similar url to test with, make sure your local server is running as http://localhost:55961:
https://accounts.google.com/o/oauth2/auth?redirect_uri=http://localhost:55961/home/OAuthGoogle&response_type=code&client_id=553327898639-2ngq3i7eve550c7et33b2789i9fseh67.apps.googleusercontent.com&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fplus.me


You need the file PhoneGameWebApi\HiddenSettings.config to override your OAuth userids and OAuth client secrets for OAuth to work.  You can override the fake appSettings keys in the Web.config- look at the example below.

For Google OAuth (PhoneGameWebApi\HiddenSettings.config)
```
<?xml version="1.0"?>
<appSettings>
  <add key="GoogleAppID" value="id" />
  <add key="GoogleAppSecret" value="secret" />
</appSettings>
```
