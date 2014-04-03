(function () {
  angular.module('oauth', [])
  .service('authentication', ['$http', '$q', function ($http, $q) {

    var baseUrl = 'http://54.200.69.198/phonegameservice';

    this.getOAuthUrl = function (provider) {
      var deferred = $q.defer();
      $http({ method: 'GET', url: baseUrl+'/api/authorization/GetOAuthUrl/' + provider })
      .success(function (data, status, headers, config) {
        //Trim the quotes around the url
        if (data.substr(0, 1) === '"') {
          data = data.substring(1);
        }
        var len = data.length;
        if (data.substr(len - 1, 1) === '"') {
          data = data.substring(0, len - 1);
        }
        deferred.resolve(data);
      })
      .error(function (data, status, headers, config) {
        deferred.reject("Couldn't get OAuth url");
      });

      return deferred.promise;
    }
    this.openOAuthWindow = function (provider) {

      var deferred = $q.defer();

      this.getOAuthUrl(provider).then(function(data){
        var authWindow = window.open(data, "_blank", 'location=no,toolbar=no');
        var returnedFromProvider = false;

        authWindow.addEventListener('loadstart', function (e) {
          var url = e.url;
          var code = /\?code=(.+)$/.exec(url);
          var error = /\?error=(.+)$/.exec(url);

          if (code) {
            returnedFromProvider = true;
            authWindow.close();
            $http({ method: 'GET', url: baseUrl+'/api/authorization/token/'+provider+'/?oauthcode=' + code[1] })
            .success(function (data, status, headers, config) {
              deferred.resolve(data);
              resolved = true;
            })
            .error(function (data, status, headers, config) {
              deferred.reject("OAuth failed at server");
              resolved = true;
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
      }, function (notification) { });

      return deferred.promise;
    }

    this.isLoggedIn = function () {
      var credentials = window.localStorage['credentials'];
      if (credentials === null || credentials === undefined)
        return false;
      else
        return true;      
    }
    this.getCredentials = function () {
      return JSON.parse(window.localStorage['credentials']);
    }
    this.saveCredentials = function (credentials) {
      window.localStorage['credentials'] = JSON.stringify(credentials);
    }
    this.logUserOut = function(){
      window.localStorage.removeItem('credentials');
    }
    this.getOAuthHeaders = function () {
      var credentials = this.getCredentials();
      return {
        'oauth_encrypted_token': credentials.oauth_encrypted_token,
        'oauth_provider': credentials.oauth_provider,
        'phone_game_id': credentials.phone_game_id
      };
    }
  }]);
})();