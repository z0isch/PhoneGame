(function () {
  'use strict';
  angular.module('app').controller('ChoosePlayerCtrl', ['$scope', '$ionicPlatform', '$http',
    '$state', '$ionicLoading', '$ionicNavBarDelegate', 'contacts', 'playersService', 'gameService',
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
})();