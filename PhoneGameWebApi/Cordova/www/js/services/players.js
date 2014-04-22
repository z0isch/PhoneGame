(function () {
  'use strict';
  angular.module('phonegame').service('playersService', ['$http', '$q', 'authenticationService', 'baseUrl',
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
})();