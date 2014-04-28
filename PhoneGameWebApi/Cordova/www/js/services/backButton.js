(function () {
  'use strict';
  angular.module('phonegame').service('backButton', ['$document', function ($document) {
      var self = this;
		  self.setHandler = function (handler) {
		    $document.unbind("backbutton");
		    $document.bind("backbutton", function (e) {
		      handler();
		      e.preventDefault();
		      e.stopPropagation();
		      return false;
		    });
		  }
		}
	])
})();