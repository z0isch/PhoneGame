(function () {
  angular.module('app', ['ionic', 'oauth', 'phonegame','cordova.contacts'])
    .config(function ($stateProvider, $urlRouterProvider) {
      $stateProvider
      .state('login', {
        url: '/login',
        templateUrl: 'login.html',
        controller: 'LoginCtrl'
      })
      .state('main', {
        url: '/',
        templateUrl: 'main.html',
        controller: 'MainCtrl'
      })
      .state('choosePlayer', {
        url: '/choosePlayer',
        templateUrl: 'choosePlayer.html',
        controller: 'ChoosePlayerCtrl'
      });

      $urlRouterProvider.otherwise("/");

    })
    .controller('LoginCtrl', ['$scope', '$ionicPlatform', '$http', 'authenticationService', '$state', '$ionicLoading','$ionicNavBarDelegate',
      function ($scope, $ionicPlatform, $http, authenticationService, $state, $ionicLoading, $ionicNavBarDelegate) {
        $ionicPlatform.ready(function () {
          $scope.googleLogin = function () {
            var promise = authenticationService.openOAuthWindow("google");
            var loadingScreen = $ionicLoading.show({
              content: 'Logging in...',
            });
            promise.then(function (credentials) {
              authenticationService.saveCredentials(credentials);
              $state.go('main');
            }, function (error) {
              alert(error);
            });

            promise['finally'](function () {
              loadingScreen.hide();
            });
            
          }
        });
      }
    ])
    .controller('MainCtrl', ['$scope', '$ionicPlatform', '$http', '$state', 'authenticationService', 'playersService', '$ionicLoading','$ionicNavBarDelegate',
      function ($scope, $ionicPlatform, $http, $state, authenticationService, playersService, $ionicLoading, $ionicNavBarDelegate) {
        $scope.loggedIn = false;
        $scope.inGames = false;

        $scope.logOut = function () {
          authenticationService.logUserOut();
          $scope.loggedIn = false;
        }

        $ionicPlatform.ready(function () {
          if (authenticationService.isLoggedIn()) {
            var credentials = authenticationService.getCredentials();
            $scope.loggedIn = true;

            var loadingScreen = $ionicLoading.show({
              content: 'Getting account info...',
            });

            var promise = playersService.getPlayer(credentials.phone_game_id);
            promise.then(function (player) {
              $scope.name = player.Name;
              $scope.inGames = false;
              $scope.games = [];
            }, function (error) {
              alert(error);
            });

            promise['finally'](function () {
              loadingScreen.hide();
            });
            
          }
        });
      }
    ])
    .controller('ChoosePlayerCtrl', ['$scope', '$ionicPlatform', '$http', '$state', '$ionicLoading', '$ionicNavBarDelegate','contacts','playersService',
      function ($scope, $ionicPlatform, $http, $state, $ionicLoading, $ionicNavBarDelegate, contacts, playersService) {

        $scope.playerPicked = function (player) {
        }

        $ionicPlatform.ready(function () {
          var loadingScreen = $ionicLoading.show({
            content: 'Getting contacts...',
          });

          var promise = contacts.find();
          promise.then(function (contacts) {
            playersService.getPlayersFromCordovaContacts(contacts).then(function (players) {
              var registeredContacts = [],
                unRegisteredContacts = [];

              contacts.forEach(function (contact) {
                if (contact.phoneNumbers) {
                  var registered = contact.phoneNumbers.filter(function (number) {
                    return players[number.value] !== undefined;
                  }).length > 0;

                  if (registered)
                    registeredContacts.push(contact);
                  else
                    unRegisteredContacts.push(contact);
                }
              });

              $scope.registeredContacts = registeredContacts;
              $scope.unRegisteredContacts = unRegisteredContacts;
            });
          }, function (error) {
            alert(error);
          });

          promise['finally'](function () {
            loadingScreen.hide();
          });

        });
      }
    ]);
})();
