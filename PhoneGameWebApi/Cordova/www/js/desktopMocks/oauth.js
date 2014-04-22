(function () {
  'use strict';
  angular.module('oauth', ['phonegame'])
  .service('authenticationService', ['$http', '$q', 'authorizationService', '$timeout',
    function ($http, $q, authorizationService, $timeout) {
      this.openOAuthWindow = function (provider) {
        var deferred = $q.defer();

        authorizationService.getOAuthUrl("TestProvider").then(function (data) {
          var testUser = prompt("Which test user? (1,2, etc.)");
          authorizationService.getToken("TestProvider", testUser).then(function (data) {
            deferred.resolve(data);
          });
        });

        return deferred.promise;
      }

      this.isLoggedIn = function () {
        var credentials = window.localStorage.getItem('credentials');
        if (credentials === null || credentials === undefined)
          return false;
        else
          return true;
      }
      this.getCredentials = function () {
        return JSON.parse(window.localStorage.getItem('credentials'));
      }
      this.saveCredentials = function (credentials) {
        window.localStorage.setItem('credentials', JSON.stringify(credentials));
      }
      this.logUserOut = function () {
        window.localStorage.removeItem('credentials');
      }
      this.getOAuthHeaders = function () {
        if (this.isLoggedIn()) {
          var credentials = this.getCredentials();
          return {
            'oauth_encrypted_token': credentials.oauth_encrypted_token,
            'oauth_provider': credentials.oauth_provider,
            'phone_game_id': credentials.phone_game_id
          };
        }
        else {
          return {};
        }
      }
    }
  ]);
})();