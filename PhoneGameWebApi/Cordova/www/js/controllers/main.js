﻿(function () {
  'use strict';
  angular.module('app').controller('MainCtrl', ['$scope', '$ionicPlatform', '$http', '$state', 'authenticationService',
    'playerService', '$ionicLoading', '$ionicNavBarDelegate', 'gameService',
    function ($scope, $ionicPlatform, $http, $state, authenticationService, playerService,
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

          var promise = playerService.getPlayer(credentials.phone_game_id);
          promise.then(function (player) {
            $scope.name = player.Name;
            var gamePromise = gameService.getGames(credentials.phone_game_id).then(function (data) {
              if (data.length > 0) {
                $scope.inGames = true;
                $scope.yourTurnGames = data.filter(function (g) {
                  return g.ActivePlayer.ID === credentials.phone_game_id;
                });
                $scope.notYourTurnGames = data.filter(function (g) {
                  return g.ActivePlayer.ID !== credentials.phone_game_id;
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
})();