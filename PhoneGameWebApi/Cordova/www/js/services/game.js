(function () {
  'use strict';
  angular.module('phonegame').service('gameService', ['$http', '$q', 'authenticationService', 'baseUrl',
    function ($http, $q, authenticationService, baseUrl) {
      this.startNewGameWith = function (phoneGameId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.post(baseUrl + '/api/games',
          { playerIds: [h.phone_game_id, phoneGameId] },
          { headers: h, responseType: 'json' })
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
      this.getGames = function (playerId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.get(baseUrl + '/api/players/' + playerId + "/games/", { headers: h, responseType: 'json' })
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
      this.getGame = function (gameId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.get(baseUrl + '/api/games/' + gameId, { headers: h, responseType: 'json' })
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
      this.deleteGame = function (gameId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.delete(baseUrl + '/api/games/' + gameId, { headers: h, responseType: 'json' })
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