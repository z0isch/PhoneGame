(function () {
  'use strict';
  angular.module('phonegame').service('backButton', ['$document','$window', function ($document,$window) {
    var self = this;
    self.setHandler = function (handler) {
      $document.unbind("backbutton");
      $document.bind("backbutton", handler);
    }

    self.useDefaultHandler = function () {
      $document.unbind("backbutton");
      $document.bind("backbutton", function(){
        $window.history.back();
      });
    }

    self.triggerHandler = function () {
      $document.triggerHandler('backbutton');
    }
  }])
})();