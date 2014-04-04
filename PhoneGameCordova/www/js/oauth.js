(function () {
  angular.module('oauth', ['phoneGame'])
  .service('authentication', ['$http', '$q','authorizationService',
    function ($http, $q, authorizationService) {
      this.openOAuthWindow = function (provider) {

        var deferred = $q.defer();

        authorizationService.getOAuthUrl(provider).then(function (data) {

          var authWindow = window.open(data, "_blank", 'location=no,toolbar=no');
          var returnedFromProvider = false;

          authWindow.addEventListener('loadstart', function (e) {
            var url = e.url;
            var code = /\?code=(.+)$/.exec(url);
            var error = /\?error=(.+)$/.exec(url);

            if (code) {
              returnedFromProvider = true;
              authWindow.close();
              authorizationService.getToken(provider, code[1]).then(function(data) {
                deferred.resolve(data);
              }, function (error) {
                deferred.reject(error);
              });
            }
            if (error) {
              returnedFromProvider = true;
              authWindow.close();
              deferred.reject("OAuth failed at provider");
            }
          });
          authWindow.addEventListener('exit', function (e) {
            if (!returnedFromProvider) deferred.reject("User closed window");
          });
        },function(error){
          deferred.reject(error);
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
      this.logUserOut = function(){
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