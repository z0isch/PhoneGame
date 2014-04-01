// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('starter', ['ionic'])

.run(function($ionicPlatform) {
  $ionicPlatform.ready(function() {
    if(window.StatusBar) {
      StatusBar.styleDefault();
    }
    var authWindow = window.open("https://accounts.google.com/o/oauth2/auth?redirect_uri=http%3a%2f%2flocalhost%3a55961%2fhome%2fGoogleOAuth&response_type=code&client_id=553327898639-2ngq3i7eve550c7et33b2789i9fseh67.apps.googleusercontent.com&scope=profile",
         "_blank", 'location=no,toolbar=no');
    authWindow.addEventListener('loadstart', function (e) {
      var url = e.url;
      var code = /\?code=(.+)$/.exec(url);
      var error = /\?error=(.+)$/.exec(url);
      if (code || error) {
        authWindow.close();
        alert(code[1]);
      }
      
    });
  });
})
