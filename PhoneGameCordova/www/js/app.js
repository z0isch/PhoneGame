// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
angular.module('PhoneGame', ['ionic'])
.controller('LoginCtrl', function ($scope, $ionicPlatform, $http) {
  $scope.loggedIn = false;
  $scope.status='<h3>You are not logged in!</h3><p>Please use an OAuth provider below:</p>';
  $ionicPlatform.ready(function () {
    if (window.StatusBar) {
      StatusBar.styleDefault();
    }
    $scope.googleLogin = function () {
        var authWindow = window.open("https://accounts.google.com/o/oauth2/auth?redirect_uri=http://localhost&response_type=code&client_id=553327898639-hasp38g0chplhbvq0hb5nt652pkbsgqd.apps.googleusercontent.com&scope=profile",
           "_blank", 'location=no,toolbar=no');
      authWindow.addEventListener('loadstart', function (e) {
        var url = e.url;
        var code = /\?code=(.+)$/.exec(url);
        var error = /\?error=(.+)$/.exec(url);

        if (code || error) {
          authWindow.close();
          $scope.status = '<h3>You have gotten a code from Google!</h3>';
          $scope.code = code[1];
          $scope.loggedIn = true;
          $scope.$apply();
          $http({ method: 'GET', url: 'http://54.200.69.198/phonegameservice/api/authorization/token/google/?oauthcode=' + $scope.code })
          .success(function (data, status, headers, config) {
            $scope.token = JSON.stringify(data);
            $scope.$apply();
            $http({
              method: 'GET', url: 'http://54.200.69.198/phonegameservice/api/authorization/tryAuthentication/', headers: {
                'oauth_encrypted_token': data.oauth_encrypted_token,
                'oauth_provider': data.oauth_provider,
                'phone_game_id': data.phone_game_id
              }
            })
            .success(function (data, status, headers, config) {
              $scope.identity = data;
              $scope.$apply();
            })
            .error(function (data, status, headers, config) {
              alert("Error: " + status);
            })
          })
          .error(function (data, status, headers, config) {
            alert("Error: " + status);
           });
        }

      });
    }
  });
});
