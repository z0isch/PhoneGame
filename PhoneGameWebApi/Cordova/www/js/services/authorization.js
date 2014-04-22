(function () {
  'use strict';
  angular.module('phonegame').service('authorizationService', ['$http', '$q', 'baseUrl',
  function ($http, $q, baseUrl) {

    this.getToken = function (provider, code) {
      var deferred = $q.defer();
        
      $http.get(baseUrl + '/api/authorization/token/' + provider + '/?oauthcode=' + code)
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
      $http.get(baseUrl + '/api/authorization/GetOAuthUrl/' + provider)
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