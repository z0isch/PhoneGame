(function () {
  'use strict';
  angular.module('phonegame').service('phraseService', ['$http', '$q', 'authenticationService', 'baseUrl',
    function ($http, $q, authenticationService, baseUrl) {
      this.getPhrases = function () {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.get(baseUrl + '/api/phrases', { headers: h, responseType: 'json' })
        .success(function (data, status, headers, config) {
          deferred.resolve(data);
        })
        .error(function (data, status, headers, config) {
          if (status == "401")
            deferred.reject("User not logged in");
          else
            deferred.reject("Server error with: " + status);
        });

        return deferred.promise;
      }
      this.pickPhrase = function (gameId, phraseId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.post(baseUrl + '/api/games/' + gameId + "/phrases/" + phraseId, { headers: h, responseType: 'json' })
        .success(function (data, status, headers, config) {
          deferred.resolve(data);
        })
        .error(function (data, status, headers, config) {
          if (status == "401")
            deferred.reject("User not logged in");
          else
            deferred.reject("Server error with: " + status);
        });

        return deferred.promise;
      }
    }
  ])
})();