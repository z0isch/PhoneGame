(function () {
  angular.module('phonegame', ['oauth', 'phonegame.url'])
  .service('phraseService', ['$http', '$q', 'authenticationService', 'baseUrl',
    function ($http, $q, authenticationService, baseUrl) {
      this.getPhrases = function () {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.get(baseUrl + '/api/phrases',{ headers: h, responseType: 'json' })
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

        $http.post(baseUrl + '/api/games/'+gameId+"/phrases/"+phraseId, { headers: h, responseType: 'json' })
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
  .service('gameService', ['$http', '$q', 'authenticationService', 'baseUrl',
    function ($http, $q, authenticationService, baseUrl) {
      this.startNewGameWith = function (phoneGameId) {
        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();

        $http.post(baseUrl + '/api/games',
          {playerIds:[h.phone_game_id, phoneGameId]},
          { headers: h, responseType: 'json'})
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

        $http.get(baseUrl + '/api/players/' + playerId+"/games/", { headers: h, responseType: 'json' })
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

        $http.get(baseUrl + '/api/games/'+gameId,{ headers: h, responseType: 'json' })
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
  .service('playersService', ['$http', '$q', 'authenticationService', 'baseUrl',
    function ($http, $q, authenticationService, baseUrl) {

      this.getPlayersFromCordovaContacts = function (contacts) {

        var deferred = $q.defer();
        var h = authenticationService.getOAuthHeaders();
        
        var phoneNumbers = contacts.reduce(function (previousValue, currentValue) {
          if (currentValue.phoneNumbers) {
            var innerNumbers = currentValue.phoneNumbers.reduce(function (previousValue, currentValue) {
              previousValue.push(currentValue.value);
              return previousValue;
            }, []);
            return previousValue.concat(innerNumbers);
          }
          else {
            return previousValue;
          }
        },[])

        var queryPhoneNumbers = phoneNumbers.reduce(function (previousValue, currentValue, index) {
          return previousValue + "phoneNumbers[" + index + "]=" + encodeURIComponent(currentValue) + "&";
        }, "");

        $http.get(baseUrl + '/api/playersFromPhoneNumbers?' + queryPhoneNumbers, { headers: h, responseType: 'json' })
        .success(function (data, status, headers, config) {
          var players = [];

          contacts.forEach(function (contact) {
            if (contact.phoneNumbers) {
              var registered = contact.phoneNumbers.filter(function (number) {
                return data[number.value] !== undefined;
              });
              if (registered[0]) {
                contact.phoneGameId = data[registered[0].value];
                contact.registered = true;
              }
              else {
                contact.registered = false;
              }
              players.push(contact);
            }
          });
          deferred.resolve(players);
        })
       .error(function (data, status, headers, config) {
         if (status == "401")
           deferred.reject("User not logged in");
         else
           deferred.reject("Server error with: " + status);
       });
        return deferred.promise;
      }

      this.getPlayer = function (id) {
        var deferred = $q.defer();
        
        var h = authenticationService.getOAuthHeaders();
        $http.get(baseUrl + '/api/players/' + id, { headers: h, responseType: 'json' })
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
  .service('authorizationService', ['$http', '$q', 'baseUrl',
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