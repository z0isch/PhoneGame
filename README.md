PhoneGame
=========
You need the file PhoneGameWebApi\HiddenSettings.config to override your OAuth userids and OAuth client secrets for OAuth to work.

For Google OAuth this looks like:
<?xml version="1.0"?>
<appSettings>
  <add key="GoogleAppID" value="id" />
  <add key="GoogleAppSecret" value="secret" />
</appSettings>
