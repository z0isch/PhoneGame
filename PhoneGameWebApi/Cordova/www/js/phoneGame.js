(function () {
  angular.module('phoneGame', ['oauth','phoneGameUrl'])
  .service('playersService', ['$http', '$q', 'authenticationService', 'phoneGameBaseUrl',
    function ($http, $q, authenticationService, phoneGameBaseUrl) {

      this.getPlayer = function (id) {
        var deferred = $q.defer();
        
        var h = authenticationService.getOAuthHeaders();
        $http.get(phoneGameBaseUrl + '/api/players/' + id, { headers: h, responseType:'json' })
         .success(function (data, status, headers, config) {
           deferred.resolve(data);
         })
        .error(function (data, status, headers, config) {
          if (status == "401")
            deferred.reject("User not logged in");
          else if (status == "404")
            deferred.reject("User not found");
          else
            deferred.reject("Server error with: "+status);
        });

        return deferred.promise;
      }
    }
  ])
  .service('authorizationService',['$http', '$q','phoneGameBaseUrl',
    function ($http, $q, phoneGameBaseUrl) {

      this.getToken = function (provider, code) {
        var deferred = $q.defer();
        
        $http.get(phoneGameBaseUrl + '/api/authorization/token/' + provider + '/?oauthcode=' + code)
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
        $http.get(phoneGameBaseUrl + '/api/authorization/GetOAuthUrl/' + provider)
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