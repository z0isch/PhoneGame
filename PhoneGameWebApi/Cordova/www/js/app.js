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
      })
      .state('pickPhrase', {
        url: '/pickPhrase/{gameId}',
        templateUrl: 'pickPhrase.html',
        controller: 'PickPhraseCtrl'
      })
      .state('game', {
        url: '/game/{gameId}',
        templateUrl: 'game.html',
        controller: 'GameCtrl'
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
    .controller('MainCtrl', ['$scope', '$ionicPlatform', '$http', '$state', 'authenticationService',
      'playersService', '$ionicLoading', '$ionicNavBarDelegate', 'gameService',
      function ($scope, $ionicPlatform, $http, $state, authenticationService, playersService,
       $ionicLoading, $ionicNavBarDelegate, gameService) {
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
              var gamePromise = gameService.getGames(credentials.phone_game_id).then(function (data) {
                if (data.length > 0) {
                  $scope.inGames = true;
                  $scope.yourTurnGames = data.filter(function (g) {
                    return g.YourTurn;
                  }).map(function (g) {
                    return g.Game;
                  });
                  $scope.notYourTurnGames = data.filter(function (g) {
                    return !g.YourTurn;
                  }).map(function (g) {
                    return g.Game;
                  });
                }
                else {
                  $scope.inGames = false;
                }
              }, function (error) {
                alert(error);
              });
              gamePromise['finally'](function () {
                loadingScreen.hide();
              });

            }, function (error) {
              alert(error);
              loadingScreen.hide();
            });            
          }
        });
      }
    ])
    .controller('ChoosePlayerCtrl', ['$scope', '$ionicPlatform', '$http',
      '$state', '$ionicLoading', '$ionicNavBarDelegate', 'contacts', 'playersService','gameService',
      function ($scope, $ionicPlatform, $http, $state, $ionicLoading, $ionicNavBarDelegate, contacts, playersService, gameService) {

        $scope.playerPicked = function (player) {
          if (player.registered) {

            var loadingScreen = $ionicLoading.show({
              content: 'Creating Game...',
            });
            var startGamePromise = gameService.startNewGameWith(player.phoneGameId).then(function (data) {
              $state.go('pickPhrase', { gameId: data.ID });
            }, function (error) {
              alert(error);
            });
            startGamePromise['finally'](function () {
              loadingScreen.hide();
            });
          }

          else
            alert("Sorry the other player must be registered!");
        }

        $ionicPlatform.ready(function () {
          var loadingScreen = $ionicLoading.show({
            content: 'Getting contacts...',
          });

          var promise = contacts.find();
          promise.then(function (contacts) {
            var getPlayers = playersService.getPlayersFromCordovaContacts(contacts);
            getPlayers.then(function (players) {
              $scope.registeredContacts = players.filter(function (p) {
                return p.registered;
              });
              $scope.unRegisteredContacts = players.filter(function (p) {
                return !p.registered;
              });;
            });

            getPlayers['finally'](function () {
              loadingScreen.hide();
            });

          }, function (error) {
            alert(error);
            loadingScreen.hide();
          });
        });
      }
    ])
    .controller('PickPhraseCtrl', ['$scope', '$ionicPlatform', '$http',
      '$state', '$stateParams','$ionicLoading', '$ionicNavBarDelegate','phraseService',
      function ($scope, $ionicPlatform, $http, $state,$stateParams, $ionicLoading, $ionicNavBarDelegate, phraseService) {
        $scope.phrasePicked = function(phrase) {
          var loadingScreen = $ionicLoading.show({
            content: 'Choosing Phrase...',
          });
          var promise = phraseService.pickPhrase($stateParams.gameId, phrase.id).then(function (data) {
            $state.go('game', { gameId: data.ID });
          }, function (error) {
            alert(error);
          });
          promise['finally'](function () {
            loadingScreen.hide();
          });
        }
        var loadingScreen = $ionicLoading.show({
          content: 'Getting Phrases...',
        });
        var promise = phraseService.getPhrases().then(function (data) {
          $scope.phrases = data;
        }, function (error) {
          alert(error);
        });
        promise['finally'](function () {
          loadingScreen.hide();
        });
      }
    ])
    .controller('GameCtrl', ['$scope', '$ionicPlatform', '$http',
        '$state', '$stateParams', '$ionicLoading', '$ionicNavBarDelegate', 'gameService',
        function ($scope, $ionicPlatform, $http, $state, $stateParams, $ionicLoading, $ionicNavBarDelegate, gameService) {
          var loadingScreen = $ionicLoading.show({
            content: 'Getting Game...',
          });
          var promise = gameService.getGame($stateParams.gameId).then(function (data) {
            $scope.game = data;
          }, function (error) {
            alert(error);
          });
          promise['finally'](function () {
            loadingScreen.hide();
          });
        }
    ]);
})();
