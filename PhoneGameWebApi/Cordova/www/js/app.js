(function () {
  'use strict';
  angular.module('app', ['ionic', 'oauth', 'phonegame', 'cordova.contacts'])
      .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
        .state('login', {
          url: '/login',
          templateUrl: 'templates/login.html',
          controller: 'LoginCtrl'
        })
        .state('main', {
          url: '/',
          templateUrl: 'templates/main.html',
          controller: 'MainCtrl'
        })
        .state('choosePlayer', {
          url: '/choosePlayer',
          templateUrl: 'templates/choosePlayer.html',
          controller: 'ChoosePlayerCtrl'
        })
        .state('pickPhrase', {
          url: '/pickPhrase/{gameId}',
          templateUrl: 'templates/pickPhrase.html',
          controller: 'PickPhraseCtrl'
        })
        .state('game', {
          url: '/game/{gameId}',
          templateUrl: 'templates/game.html',
          controller: 'GameCtrl'
        });
        $urlRouterProvider.otherwise("/");
      });
})();