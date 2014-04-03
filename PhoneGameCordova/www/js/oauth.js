(function () {
  angular.module('oauth', [])
  .service('authentication', ['$http', '$q', function ($http, $q) {
    this.openOAuthWindow = function (type) {
      var deferred = $q.defer();
      var authWindow = window.open("https://accounts.google.com/o/oauth2/auth?redirect_uri=http://localhost&response_type=code&client_id=553327898639-hasp38g0chplhbvq0hb5nt652pkbsgqd.apps.googleusercontent.com&scope=profile", "_blank", 'location=no,toolbar=no');
      var returnedFromProvider = false;

      authWindow.addEventListener('loadstart', function (e) {
        var url = e.url;
        var code = /\?code=(.+)$/.exec(url);
        var error = /\?error=(.+)$/.exec(url);

        if (code) {
          returnedFromProvider = true;
          authWindow.close();
          $http({ method: 'GET', url: 'http://54.200.69.198/phonegameservice/api/authorization/token/google/?oauthcode=' + code[1] })
          .success(function (data, status, headers, config) {
            deferred.resolve(data);
            resolved = true;
          })
          .error(function (data, status, headers, config) {
            deferred.reject("Google auth failed at server");
            resolved = true;
          });
        }
        if (error) {
          returnedFromProvider = true;
          authWindow.close();
          deferred.reject("Google auth failed at provider");
        }
      });
      authWindow.addEventListener('exit', function (e) {
        if (!returnedFromProvider) deferred.reject("User closed window");
      });
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
  }]);
})();