(function () {
  angular.module('phoneGame', ['oauth'])
  .constant('phoneGameBaseUrl', 'http://54.200.69.198/phonegameservice')
  .service('playersService', ['$http', '$q', 'authentication', 'phoneGameBaseUrl',
    function ($http, $q, authentication, phoneGameBaseUrl) {
      this.getPlayer = function (id) {
        var deferred = $q.defer();
        alert(JSON.stringify(authentication.getOAuthHeaders()));
        $http({
          method: 'GET',
          url: phoneGameBaseUrl+'api/Players/' + id,
          headers: authentication.getOAuthHeaders()
        })
         .success(function (data, status, headers, config) {
           deferred.resolve(data);
         })
        .error(function (data, status, headers, config) {
          if(status == "404")
            deferred.reject("User not logged in");
          else if(status == "400")
            deferred.reject("User not found");
        })
        return deferred.promise;
      }
    }
  ])
  .service('authorizationService',['$http', '$q','phoneGameBaseUrl',
    function ($http, $q, phoneGameBaseUrl) {

      this.getToken = function (provider, code) {
        var deferred = $q.defer();
        
        $http({ method: 'GET', url: phoneGameBaseUrl + '/api/authorization/token/' + provider + '/?oauthcode=' + code })
        .success(function (data, status, headers, config) {
          deferred.resolve(data);
        })
        .error(function (data, status, headers, config) {
          deferred.reject("OAuth failed at server");
        });

        return deferred.promise;
      }

      this.getOAuthUrl = function(provider) {
        var deferred = $q.defer();
        $http({ method: 'GET', url: phoneGameBaseUrl + '/api/authorization/GetOAuthUrl/' + provider })
        .success(function (data, status, headers, config) {
          //Trim the quotes around the url
          if (data.substr(0, 1) === '"') data = data.substring(1);
          if (data.substr(data.length - 1, 1) === '"') data = data.substring(0, data.length - 1);
          deferred.resolve(data);
        })
        .error(function (data, status, headers, config) {
          deferred.reject("Couldn't get OAuth url");
        });

        return deferred.promise;
      }
    }
  ]);
})();