'use strict';

angular.module('crossroads',
    [
      'crdsProfile'
    ]

).config(function() {

});


angular.module('crdsProfile', []);


'use strict';
var $buoop = {}; 
$buoop.ol = window.onload; 
window.onload=function(){ 
 try {if ($buoop.ol) $buoop.ol();}catch (e) {} 
 var e = document.createElement('script'); 
 e.setAttribute('type', 'text/javascript'); 
 e.setAttribute('src', '//browser-update.org/update.js'); 
 document.body.appendChild(e); 
}; 


'use strict';

angular.module('crdsProfile')

.controller('crdsProfileHousehold',
  function($scope, $filter, Profile, Congregations, States, Countries) {

  Congregations.web().then(function(congregations) {
    $scope.congregations = congregations;
  });

  States.all().then(function(states) {
    $scope.states = states;
  });

  Countries.all().then(function(countries) {
    $scope.countries = countries;
  });

  $scope.saveHousehold = function() {
    Profile.saveHousehold($scope.household);
    Profile.saveAddress($scope.address);
  };

  $scope.setCountry = function() {
    $scope.address.foreignCountry = $scope.countryObj.country;
    $scope.address.countryCode = $scope.countryObj.code3;
  };
});


'use strict';

angular.module('crossroads')

.controller('AppCtrl', function($rootScope, $log, growl) {

  $rootScope.$on('notify.success', function(event, message) {
    growl.success(lookup(message));
  });

  $rootScope.$on('notify.info', function(event, message) {
    growl.info(lookup(message));
  });

  $rootScope.$on('notify.warning', function(event, message) {
    growl.warning(lookup(message));
  });

  $rootScope.$on('notify.error', function(event, message) {
    growl.error(lookup(message));
  });

  var lookup = function(messageKey) {
    switch (messageKey) {
      case 'form.success':
        return 'Your request has been submitted successfully';
      case 'form.validation.error':
        return 'Your request has errors';
      case 'form.server.error':
        return 'An error has occurred';
      default:
        return messageKey;
    }
  };
});


'use strict';

angular.module('crossroads')

.controller('crdsMenuCtrl',
    function($scope, $rootScope, Menu, SecurityContext) {

  $scope.menu = Menu;
  $scope.loginShow = false;

  $scope.logout = function() {
    SecurityContext.logout();
  };

  $scope.toggleDesktopLogin = function() {
    if (this.loginShow === true) {
      this.loginShow = false;
    } else {
      this.loginShow = true;
    }
  };

  $scope.toggleMenuDisplay = function() {
    this.menu.toggleMobileDisplay();
  };

  $rootScope.$on('login:hide', function() {
    $scope.loginShow = false;
  });
});


'use strict';

angular.module('crossroads')

.controller('LoginCtrl', function($scope, SecurityContext) {

  $scope.user = {};

  $scope.login = function() {
    var promise = SecurityContext.login($scope.user.username,
                                        $scope.user.password);

    if (this.user.username && this.user.password) {
      $scope.processing = 'Logging in...';

      promise.then(function() {
        $scope.loginError = void 0;
      }, function() {
        $scope.processing = null;
        $scope.loginError = ['Oops!', 'Login failed.'];
      });
    } else {
      $scope.loginError = ['Hold up!', 'Username/password can\'t be blank'];
    }
  };
});


'use strict';

angular.module('crossroads')

.controller('MainCtrl', function(Menu) {

  this.menus = Menu.menuData;

  this.toggleMenu = function() {
    Menu.toggleMobileDisplay();
  };
});


'use strict';

angular.module('crossroads')

.controller('mediaPosterCtrl', function($element) {

    // showPoster will hide the child img & svg tags, 
    // then find any YouTube embeds (iframes) that are in the parent element and show it.
    // Based on logic from here: https://gist.github.com/zigotica/4438876

    this.showPoster = function() {
      var elm = $element,
          conts   = elm.contents(),
          le      = conts.length,
          ifr     = null;

      for(var i = 0; i<le; i++){
        if(conts[i].nodeType === 8) ifr = conts[i].textContent;
      }

      $element.addClass('player').html(ifr);
      $element.off('click');
    };
});

'use strict';

angular.module('crossroads')

.directive('authForm', function() {
  return {
    restrict: 'EA',
    templateUrl: 'crossroads.net/templates/login.html',
    controller: 'LoginCtrl',
    priority: 0
  };
});


'use strict';

angular.module('crossroads')

.directive('addThis', function() {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'crossroads.net/templates/addthis.html'
    };
});



'use strict';

angular.module('crossroads')

.directive('crdsLoadingText', function() {
  return {
    restrict: 'A',
    scope: {
      'crdsLoadingText': '@',
      'crdsLoading': '='
    },
    link: function($scope, $element, $attrs) {
      var baseText = $element.html(),
          baseDisabled = $attrs.disabled;

      $scope.$watch('crdsLoading', function(value) {
        var text;

        if (value) {
          $attrs.$set('disabled', true);
          text = $scope.$parent.$eval($scope.crdsLoadingText);

          if (!text) {
            text = $scope.crdsLoadingText;
          }

          $element.html(text);

        } else {
          $element.html(baseText);

          $attrs.$set('disabled', baseDisabled);
        }
      });

      $scope.$parent.$watch($attrs.ngDisabled, function(value) {
        baseDisabled = value;
      });
    }
  };
});


'use strict';

angular.module('crossroads').directive('crdsProgressBar', function() {
    return {
        restrict: 'E',
        templateUrl: 'crossroads.net/templates/crdsProgressbar.html',
        scope: {
            percentage: '@'
        }
    };
});


'use strict';

angular.module('crossroads')

.directive('crdsMenu', function() {
  return {
    controller: 'crdsMenuCtrl',
    templateUrl: 'crossroads.net/templates/crdsMenu.html',
    require: '?authForm',
    priority: 99,
    scope: {
      menus: '=crdsMenu',
      securityContext: '='
    }
  };
});


'use strict';

angular.module('crossroads')

.directive('hideOnMobileMenu', function(Menu) {
  return {
    link: function(scope, element) {
      scope.menu = Menu;

      scope.$watch('menu.isMobileShowing', function() {
        if (scope.menu.isMobileShowing) {
          element.addClass('show');
        } else {
          element.removeClass('show');
        }
      });
    }
  };
});


'use strict';

angular.module('crossroads')

.directive('randomBlock', function() {
  return {
    restrict: 'C',
    compile: function(telement) {

      var children = _.map(telement.children(), function(child) {
        return angular.element(child);
      });

      _.each(children, function(child) {
        child.addClass('hidden');
      });

      var visible = _.sample(children);
      visible.removeClass('hidden');
    }
  };
});


'use strict';

angular.element(window).bind('load', function() {
    var addThisScript = document.createElement('script');
    addThisScript.setAttribute('type', 'text/javascript');
    addThisScript.setAttribute('src', '//s7.addthis.com/js/300/addthis_widget.js#domready=1');
    document.body.appendChild(addThisScript);
    // jshint ignore:start
    var addthis_config = addthis_config||{};
    addthis_config.pubid = 'ra-5391d6a6145291c4';
    // jshint ignore:end
});


'use strict';

angular.module('crdsAuth', [])

.factory('Auth', function($http, $rootScope, $q) {
  return {
    getCurrentUser: function() {
      var deferred;
      deferred = $q.defer();
      $http.get(
        '/api/ministryplatformapi/PlatformService.svc/GetCurrentUserInfo')
        .then(function(response) {
          if (typeof response.data === 'object') {
            deferred.resolve(response.data);
          } else {
            deferred.resolve(null);
          }
        }, function(error) {
          return deferred.reject(error);
        });
      return deferred.promise;
    },

    logout: function() {
      return $http['delete']('/logout').then(function() {
        $rootScope.currentUser = null;
      });
    },

    login: function(username, password) {
      var data, deferred;
      deferred = $q.defer();
      data = {
        username: username,
        password: password
      };
      $http({
        url: '/login',
        method: 'POST',
        data: $.param(data),
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: null
        }
      }).success(function(data) {
        $rootScope.$emit('login:hide');
        return deferred.resolve(data);
      }).error(function(data, status) {
        if (status === 0) {
          console.log('Could not reach API');
          return deferred.reject('Could not reach API');
        } else {
          console.log('Invalid username & password');
          return deferred.reject('Invalid username & password');
        }
      });
      return deferred.promise;
    }
  };
});


'use strict';

angular.module('crossroads')

.factory('Menu', function($window) {
  var Menu = (function() {
    function Menu(menuData) {
      this.menuData = menuData;
    }

    Menu.prototype.toggleMobileDisplay = function() {
      this.isMobileShowing = !this.isMobileShowing;
    };

    Menu.prototype.collapsed = function(index) {
      return index !== this.selectedMenuItem;
    };

    Menu.prototype.toggleMenuItem = function(index) {
      if (this.collapsed(index)) {
        this.selectedMenuItem = index;
      } else {
        this.selectedMenuItem = null;
      }
    };

    return Menu;

  })();
  return new Menu($window.menuData);
});


'use strict';

angular.module('crdsProfile')

.controller('crdsDatePickerCtrl', function($scope) {

  $scope.today = function() {
    $scope.dt = new Date();
  };

  $scope.clear = function() {
    $scope.dt = null;
  };

  $scope.open = function($event) {
    $event.preventDefault();
    $event.stopPropagation();
    $scope.opened = true;
  };

  $scope.dateOptions = {
    formatYear: 'yy',
    startingDay: 1,
    showWeeks: false
  };

  $scope.today();
});


'use strict';

angular.module('crdsProfile')

.controller('crdsProfileHousehold',
  function($scope, $filter, Profile, Congregations, States, Countries) {

  Congregations.web().then(function(congregations) {
    $scope.congregations = congregations;
  });

  States.all().then(function(states) {
    $scope.states = states;
  });

  Countries.all().then(function(countries) {
    $scope.countries = countries;
  });

  $scope.saveHousehold = function() {
    Profile.saveHousehold($scope.household);
    Profile.saveAddress($scope.address);
  };

  $scope.setCountry = function() {
    $scope.address.foreignCountry = $scope.countryObj.country;
    $scope.address.countryCode = $scope.countryObj.code3;
  };
});


'use strict';

angular.module('crdsProfile')

.controller('crdsProfilePersonal', function($scope, Profile, MaritalStatus, Gender) {
  MaritalStatus.all().then(function(statuses) {
    $scope.maritalStatuses = statuses;
  });

  Gender.all().then(function(genders) {
    $scope.genders = genders;
  });

  $scope.savePersonal = function(data) {
    Profile.saveContact(data);
  };
});


var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

angular.module("crdsProfile").filter('DateYearFilter', function(){
  return function(items, year, field) {
    var result = [];

    if (angular.isDefined(items) && angular.isDefined(year)) {
      if(year==null) {
        result=items;
      }
      for (var i=0; i<items.length; i++) {
        if (items[i][field].getFullYear() == year) {
          result.push(items[i]);
        }
      }
    }
    return result;
  };
});

angular.module("crdsProfile").directive("crdsProfile", function($timeout, $animate, $compile, $templateCache, $filter) {
  return {
    restrict: 'EA',
    scope: true,
    controller: function($scope, $element, $attrs, Profile, Constants) {
      return new ((function() {
        function _Class() {


          this.registerLogin = __bind(this.registerLogin, this);
          var template;
          this.addLoading();

          $scope.percentage = this.calculateProfileCompleteness();
          template = $templateCache.get('profile/templates/profile.html');
          this.profileTemplate = $compile(template);
          $scope.$watch("securityContext.loggedIn", (function(_this) {
            return function(loggedIn) {
              var parent, user;
              user = $scope.securityContext.user;
              if (loggedIn && user) {
                Profile.getContact(user.ContactId).then(function(data) {
                  // Profile.getHousehold(data.householdId).then(function(data) {
                  //   Profile.getAddress(data.addressId).then(function(data) {
                  //     $scope.countryObj = [];
                  //     $scope.countryObj.dpRecordname = data.foreignCountry;
                  //     return $scope.address = data;
                  //   });
                  //   return $scope.household = data;
                  // });
                  return $scope.contact = data;
                });
                Profile.getFamily(user.ContactId).then(function(data) {
                  return $scope.family = data;
                });
                Profile.getGivingHistory().then(function(data) {
                  $scope.years = [];
                  var firstYearGiving = Math.min.apply(Math, data.map(function (el) {
                    return (el["donationDate"]).getFullYear();
                  }));
                  for (var i=$scope.dateFilter = new Date().getFullYear(); i >= firstYearGiving; i--) {
                    $scope.years.push(i);
                  }
                  return $scope.givingHistory = data;
                });
                _this.removeLoading();
                if (_this.loginCtrl) {
                  _this.loginCtrl.hide();
                }
                _this.profileElement = _this.profileTemplate($scope);
                return $element.append(_this.profileElement);
              } else if (loggedIn === false) {
                _this.removeLoading();
                parent = $element.parent();
                if (_this.profileElement) {
                  _this.profileElement.remove();
                }
                if (_this.loginCtrl) {
                  return _this.loginCtrl.show(parent, $element);
                }
              } else if (loggedIn === null) {
                if (_this.loginCtrl) {
                  _this.loginCtrl.hide();
                }
                if (_this.profileElement) {
                  _this.profileElement.remove();
                }
                return _this.addLoading();
              }
            };
          })(this));
        }

        _Class.prototype.registerLogin = function(loginCtrl) {
          return this.loginCtrl = loginCtrl;
        };

        _Class.prototype.removeLoading = function() {
          if ($attrs.loadingClass) {
            $animate.removeClass($element, $attrs.loadingClass);
          }
          return $element.html('');
        };

        _Class.prototype.addLoading = function() {
          if ($attrs.loadingClass) {
            return $animate.addClass($element, $attrs.loadingClass);
          }
        };

        _Class.prototype.calculateProfileCompleteness = function() {

           // Logic Description
           // Make constant names to match with the ministry platform keys for profile values
           // Access Constrant.weightages values using ministry platform keys
           // Add them
           var weightage = 0;
           // Sample Add logic and sample constant access to use
           weightage = Constants.weightages.crossroadsLocation + Constants.weightages.streetAddress
           return weightage
        };

        return _Class;

      })());
    }
  };
});


var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

angular.module("crdsProfile").directive("crdsProfileLogin", function($animate) {
  return {
    require: ['^crdsProfile', 'crdsProfileLogin'],
  restrict: 'EA',
  scope: true,
  terminal: true,
  priority: 1000,
  transclude: 'element',
  link: function($scope, $element, $attrs, controllers, $transclude) {
    var crdsProfileCtrl, crdsProfileLoginCtrl;
    crdsProfileCtrl = controllers[0];
    crdsProfileLoginCtrl = controllers[1];
    crdsProfileCtrl.registerLogin(crdsProfileLoginCtrl);
  },
  controller: function($scope, $element, $attrs, $transclude) {
    return new ((function() {
      function _Class() {
        this.hide = __bind(this.hide, this);
        this.show = __bind(this.show, this);
      }

      _Class.prototype.show = function(parent, after) {
        if (!this.loginElement) {
          return $transclude((function(_this) {
            return function(clone, newScope) {
              _this.loginElement = clone;
              return $animate.enter(clone, parent, after);
            };
          })(this));
        }
      };

      _Class.prototype.hide = function() {
        if (this.loginElement) {
          $animate.leave(this.loginElement);
          return this.loginElement = null;
        }
      };

      return _Class;

    })());
  }
  };
});



'use strict';

angular.module('crdsProfile')

.service('Congregations', function(thinkMinistry) {
  return {
    web: function() {
      var path = 'GetPageRecords?pageId=466';
      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },
  };
});


'use strict';
// This is a constant service where we can keep the values which we do not want
// to change and change based on the business requirements. Please ignore the
// weight names since these are subject to change based on the Ministry
// platform data.

angular.module('crdsProfile')
    .constant('Constants', {
        weightages: {
            crossroadsLocation:15,
            streetAddress:10,
            email:14,
            atleastOneSkillSelected:8,
            profileImage:20,
            birthDay:5,
            dateStartedAttending:4,
            firstName:2,
            lastName:2,
            gender:1,
            maritalStatus:1,
            mobilePhone:15,
            homePhone:2,
            employer:1
        }
    });


'use strict';

angular.module('crdsProfile')

.service('Countries', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageRecords?pageId=442').then(function(data) {
        return data;
      });
    }
  };
});


'use strict';

angular.module('crdsProfile')

.service('Gender', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageLookupRecords?pageId=311').then(function(data) {
        return data;
      });
    }
  };
});


'use strict';

angular.module('crdsProfile')

.service('MaritalStatus', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageLookupRecords?pageId=339').then(function(data) {
        return data;
      });
    }
  };
});


'use strict';

angular.module('crdsProfile')

.factory('Profile', function($http, $q, thinkMinistry) {

  return {
    saveContact: function(contact) {
      return thinkMinistry.post('UpdatePageRecord?pageId=292', contact);
    },

    getContact: function(contactId) {

      var deferred;
      deferred = $q.defer();
      $http.get(
        'http://my.crossroads.net/gateway/api/person/' + contactId)
        .then(function(response) {
          if (typeof response.data === 'object') {
            deferred.resolve(response.data);
          } else {
            deferred.resolve(null);
          }
        }, function(error) {
          return deferred.reject(error);
        });

      return deferred.promise;
    },

    saveHousehold: function(household) {
      return thinkMinistry.post('UpdatePageRecord?pageId=465', household);
    },

    getHousehold: function(householdId) {
      var path = 'GetPageRecord?pageId=465&recordId=' + householdId;

      return thinkMinistry.get(path).then(function(data) {
        return data[0];
      });
    },

    saveAddress: function(address) {
      return thinkMinistry.post('UpdatePageRecord?pageId=468', address);
    },

    getAddress: function(addressId) {
      var path = 'GetPageRecord?pageId=468&recordId=' + addressId;

      return thinkMinistry.get(path).then(function(data) {
        return data[0];
      });
    },

    getFamily: function(contactId) {
      var path = 'GetSubpageRecords?subpageId=417&parentRecordId=' + contactId;

      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },

    getGivingHistory: function() {
      var path = 'GetPageRecords?pageId=467';

      return thinkMinistry.get(path).then(function(data) {
        return data;
      });
    },
  };
});


'use strict';

angular.module('crdsProfile')

.service('States', function(thinkMinistry) {
  return {
    all: function() {
      return thinkMinistry.get('GetPageRecords?pageId=452').then(function(data) {
        return data;
      });
    }
  };
});


angular.module("templates", []).run(["$templateCache", function($templateCache) {$templateCache.put("crossroads.net/templates/addthis.html","<div class=\"social-media-buttons addthis_toolbox addthis_default_style addthis_32x32_style\">\n  <a class=\"addthis_button_facebook\">\n    <svg viewBox=\"0 0 12 32\" class=\"icon-facebook icon-social-media icon-medium\">\n      <use xlink:href=\"/icons/cr.svg#facebook\">\n    </svg>\n  </a>\n  <a class=\"addthis_button_twitter\">\n    <svg viewBox=\"0 0 32 32\" class=\"icon-twitter icon-social-media icon-medium\">\n      <use xlink:href=\"/icons/cr.svg#twitter\">\n    </svg>\n  </a>\n  <a class=\"addthis_button_email\">\n    <svg viewBox=\"0 0 32 32\" class=\"icon-mail5 icon-social-media icon-medium\">\n      <use xlink:href=\"/icons/cr.svg#mail5\">\n    </svg>\n  </a>\n</div>\n");
$templateCache.put("crossroads.net/templates/crdsMenu.html","<div class=\"container\">\n  <div class=\"row\">\n    <div class=\"col-xs-12 menu__container\">\n      <div class=\"nav\" role=\"navigation\">\n        <div class=\"side-nav col-xs-11 hidden-md hidden-lg hidden-xl\" ng-class=\"{show: menu.isMobileShowing}\">\n          <div class=\"side-nav__search input-group\">\n            <input type=\"text\" class=\"form-control\" placeholder=\"Search\">\n            <span class=\"input-group-addon\"><svg viewBox=\"0 0 32 32\" class=\"icon icon-search3\"><use xlink:href=\"/icons/cr.svg#search3\"></use></svg></span>\n          </div>\n          <ul class=\"side-nav__dropdown list-unstyled\" id=\"mobile-menu\">\n            <li class=\"side-nav__dropdown--item\" ng-repeat=\"heading in menus.headings\">\n              <a class=\"heading__title\" ng-click=\"menu.toggleMenuItem($index)\">\n                {{heading.title}}\n                <i class=\"icon--small icon--dropdown icon--right\"></i>\n              </a>\n              <ul class=\"side-nav__subnav list-unstyled\" collapse=\"menu.collapsed($index)\">\n                <li class=\"side-nav__subnav__item-container col-xs-12\">\n          		    <div ng-if=\"$first\">\n                    <div ng-if=\"!securityContext.loggedIn\">\n                			<auth-form></auth-form>\n                    </div>\n          		    </div>\n                </li>\n            		<div ng-if=\"securityContext.loggedIn\">\n                  <li class=\"side-nav__subnav--item\" role=\"presentation\" ng-repeat=\"item in heading.mobile_items\">\n                    <a class=\"heading__item\" role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                  </li>\n            		</div>\n            		<div ng-if=\"$index != 0\">\n                  <ul class=\"side-nav__subnav list-unstyled\" collapse=\"collapsed($index)\">\n                    <div ng-if=\"$first\">\n                      <div ng-if=\"!securityContext.loggedIn\">\n                        <li class=\"side-nav__subnav--item\">\n                          <auth-form></auth-form>\n                        </li>\n                      </div>\n            		    </div>\n	                  <div ng-if=\"securityContext.loggedIn\">\n                      <li class=\"side-nav__subnav--item\" role=\"presentation\" ng-repeat=\"item in heading.mobile_items\">\n                        <a class=\"heading__item\" role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                      </li>\n    		            </div>\n                    <div ng-if=\"$index != 0\">\n                      <li class=\"side-nav__subnav--item\" role=\"presentation\" ng-repeat=\"item in heading.mobile_items\">\n                        <a class=\"heading__item\" role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                      </li>\n                		</div>\n                  </ul>\n                </div>\n              </ul>\n            </li>\n          </ul>\n        </div>\n        <div class=\"navbar-header\" ng-class=\"{show: menu.isMobileShowing}\">\n          <button type=\"button\" class=\"nav__side--toggle navbar-left hidden-md hidden-lg hidden-xl\" ng-click=\"toggleMenuDisplay()\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">\n            <span class=\"sr-only\">Toggle navigation</span>\n            <span class=\"icon-bar\"></span>\n            <span class=\"icon-bar\"></span>\n            <span class=\"icon-bar\"></span>\n          </button>\n          <a class=\"navbar-brand logo\" href=\"/\"><img src=\"//crossroads-media.s3.amazonaws.com/images/logo.svg\" alt=\"crossroads\" /></a>\n          <ul class=\"navbar--login list-unstyled\">\n      	    <span id=\"current-user\" class=\"hidden-md hidden-sm hidden-xs\">{{securityContext.user.FirstName}}</span>\n      	    <li ng-show=\"securityContext.loggedIn\" class=\"hidden-md hidden-sm hidden-xs\">\n      	      <a href=\"#\" ng-click=\"logout()\">Sign Out</a>\n      	    </li>\n            <li class=\"hidden-xs hidden-sm\" data-toggle=\"dropdown\" ng-hide=\"securityContext.loggedIn\">\n              <button class=\"btn btn-primary\" id=\"form-toggle\" ng-click=\"toggleDesktopLogin()\">\n                <span>Login</span>\n              </button>\n            </li>\n            <li>\n              <ul class=\"navbar--login__dropdown list-unstyled col-md-3 soft-top soft-sides\" role=\"menu\" aria-labelledby=\"dropdownMenu1\" ng-class=\"{toggleOpen: loginShow}\">\n                <li role=\"presentation\">\n                  <auth-form></auth-form>\n                </li>\n              </ul>\n            </li>\n            <li class=\"navbar--login__mobile visible-xs visible-sm\" ng-if=\"!securityContext.loggedIn\">\n              <a class=\"btn navbar--login__button\" href=\"/myprofile\">\n                <i class=\"icon icon--no_margin icon--user\"></i>\n              </a>\n            </li>\n            <li class=\"navbar--login__mobile visible-xs visible-sm\" ng-if=\"securityContext.loggedIn\">\n              <a class=\"btn navbar--login__button\" href=\"#\" ng-click=\"logout()\">\n                <i class=\"icon icon--no_margin icon--user__filled\"></i>\n              </a>\n      	    </li>\n          </ul>\n        </div>\n        <div class=\"hidden-xs hidden-sm\">\n          <ul class=\"navbar__dropdown list-unstyled\">\n            <li class=\"navbar__dropdown--item\" ng-repeat=\"heading in menus.headings\">\n              <a class=\"navbar--heading__title\" data-toggle=\"dropdown\" href=\"#\">\n                {{heading.title}}\n              </a>\n              <ul class=\"navbar__dropdown--subnav list-unstyled col-sm-12\" role=\"menu\" aria-labeledby=\"dropdownMenu1\" ng-if=\"$first\">\n                <li ng-repeat=\"headingItem in heading.items\" ng-class=\"headingItem.items && \'navbar__dropdown--subnav--item\' || \'navbar__dropdown--subnav__list--item\'\" role=\"presentation\">\n                  <div ng-if=\"$first\">\n                    <div ng-if=\"!securityContext.loggedIn\">\n                      <auth-form></auth-form>\n                    </div>\n                    <div ng-if=\"securityContext.loggedIn\">\n                      <a ng-class=\"{heading__item: headingItem.items}\"  role=\"menuitem\" tabindex=\"-1\" href=\"{{headingItem.link}}\">{{headingItem.title}}</a>\n                      <ul class=\"navbar__dropdown--subnav__list list-unstyled col-sm-12\" role=\"menu\" aria-labeledby=\"dropdownMenu1\" ng-repeat=\"item in headingItem.items\">\n                        <li class=\"navbar__dropdown--subnav__list--item\" role=\"presentation\">\n                          <a role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                        </li>\n                      </ul>\n                    </div>\n                  </div>\n\n                  <div ng-if=\"$index != 0\">\n                    <a ng-class=\"{heading__item: headingItem.items}\"  role=\"menuitem\" tabindex=\"-1\" href=\"{{headingItem.link}}\">{{headingItem.title}}</a>\n                    <ul class=\"navbar__dropdown--subnav__list list-unstyled col-sm-12\" role=\"menu\" aria-labeledby=\"dropdownMenu1\" ng-repeat=\"item in headingItem.items\">\n                      <li class=\"navbar__dropdown--subnav__list--item\" role=\"presentation\">\n                        <a role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                      </li>\n                    </ul>\n                  </div>\n                </li>\n              </ul>\n\n              <ul class=\"navbar__dropdown--subnav list-unstyled col-sm-12\" role=\"menu\" aria-labeledby=\"dropdownMenu1\" ng-if=\"$index != 0\">\n                <li ng-repeat=\"headingItem in heading.items\" ng-class=\"headingItem.items && \'navbar__dropdown--subnav--item\' || \'navbar__dropdown--subnav__list--item\'\" role=\"presentation\">\n                  <a ng-class=\"{heading__item: headingItem.items}\"  role=\"menuitem\" tabindex=\"-1\" href=\"{{headingItem.link}}\">{{headingItem.title}}</a>\n                  <ul class=\"navbar__dropdown--subnav__list list-unstyled col-sm-12\" role=\"menu\" aria-labeledby=\"dropdownMenu1\" ng-repeat=\"item in headingItem.items\">\n                    <li class=\"navbar__dropdown--subnav__list--item\" role=\"presentation\">\n                      <a role=\"menuitem\" tabindex=\"-1\" href=\"{{item.link}}\">{{item.title}}</a>\n                    </li>\n                  </ul>\n                </li>\n              </ul>\n            </li>\n          </ul>\n        </li>\n      </ul>\n    </div>\n  </div>\n</div>\n");
$templateCache.put("crossroads.net/templates/crdsProgressbar.html","<div class=\"progress\">\n    <div class=\"progress-bar\" role=\"progressbar\" aria-valuenow=\"{{percentage}}\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width:{{percentage}}%\">\n        {{percentage}}%\n    </div>\n</div>");
$templateCache.put("crossroads.net/templates/login.html","<div class=\"navbar--login__container\">\n  <div class=\"dd_menu\">\n    <div class=\"primary\">\n      <form id=\"nav_login\" role=\"form\" ng-submit=\"login()\">\n        <div class=\"form-group\">\n          <div class=\"navbar--login__input\" ng-class=\"{\'has-error\': loginError}\">\n            <input class=\"form-control\" type=\"text\" name=\"userName\" id=\"username_nav\" placeholder=\"Email\" autocomplete=\"off\" autocapitalize=\"off\" ng-model=\"user.username\" autofocus>\n          </div>\n          <div class=\"navbar--login__input\" ng-class=\"{\'has-error\': loginError}\">\n            <input class=\"form-control\" autocomplete=\"off\" type=\"password\" name=\"password\" id=\"password_nav\" placeholder=\"Password\" autocomplete=\"off\" autocapitalize=\"off\" ng-model=\"user.password\">\n            <a class=\"nav-icon__container\" href=\"/forgotpassword\" tabindex=\"-1\">\n              <svg viewBox=\"0 0 32 32\" class=\"icon icon-question-circle icon-subtle\">\n                <use xlink:href=\"/icons/cr.svg#question-circle\"></use>\n              </svg>\n            </a>\n          </div>\n          <div>\n            <div class=\"text-danger alert alert-danger\" ng-if=\"loginError\" role=\"alert\" ><strong>{{loginError[0]}}</strong> {{loginError[1]}}</div>\n          </div>\n          <input type=\"submit\" name=\"submit_nav\" value=\"{{ processing || \'Login\' }}\" ng-class=\"{disabled: processing }\" id=\"submit_nav\" class=\"col-xs-12 btn btn-primary push-half-bottom\">\n          <div class=\"soft-half-bottom\" style=\"font-size: smaller;\">\n            <input type=\"checkbox\" name=\"remember\" id=\"remember_nav\" ng-model=\"checked\" ng-init=\"checked=true\">\n            <label class=\"remember\">Stay Logged In</label>\n          </div>\n        </div>\n      </form>\n    </div>\n  </div>\n</div>\n");
$templateCache.put("profile/templates/profile.html","<div class=\"row\">\n  <div class=\"col-sm-10 col-sm-offset-1\">\n    <h1>My Profile</h1>\n    <h2 class=\"subheading\">Hello, special you.</h2>\n    <div class=\"profile-header\">\n      <div class=\"row\">\n        <div class=\"col-xs-3 col-sm-2\">\n          <a href=\"#\" class=\"profile-pic\">\n            <img class=\"profile-image pull-left img-square img-responsive\" src=\"//crossroads-media.s3.amazonaws.com/images/avatar.svg\" alt=\"Profile Default Photo\" />\n            <span class=\"change-photo\">Change Photo</span>\n          </a>\n        </div>\n        <div class=\"profile-desc col-xs-7 col-sm-9\">\n          <h3 class=\"profile-name\">{{contact.firstName}} {{contact.lastName}}</h3>\n          <!-- Handle a null church site here with something like \"Please select your site\" currently it\'s just blank when null-->\n          <p class=\"lead site-name\"><a href=\"#Site\" ng-click=\"set.showHousehold = true\" data-toggle=\"tab\" class=\"edit-church-site\">{{household.congregationIdText}} <svg viewBox=\"0 0 32 32\" class=\"icon icon-pencil4\"><use xlink:href=\"#pencil4\"></use></svg></a></p>\n          <div class=\"col-sm-6 row\">\n              <crds-progress-bar percentage=\"{{percentage}}\"></crds-progress-bar>\n          </div>\n        </div>\n      </div>\n    </div>\n\n    <div class=\"profile-body col-sm-12 row\">\n\n      <tabset ng-init=\"set = {}\">\n        <tab heading=\"Personal\"><ng-include src=\" \'profile/templates/profile_personal.html\' \"></ng-include></tab>\n        <tab heading=\"Household\" class=\"hidden-xs\" active=\"set.showHousehold\"><ng-include src=\" \'profile/templates/profile_household.html\'\" ></ng-include></tab>\n        <tab heading=\"Account\"><ng-include src=\" \'profile/templates/profile_account.html\' \"></ng-include></tab>\n        <tab heading=\"Giving\"><ng-include src=\" \'profile/templates/profile_giving.html\' \"></ng-include></tab>\n        <tab heading=\"Skills\" class=\"hidden-xs\" active=\"set.showSkills\"><ng-include src=\" \'profile/templates/profile_skills.html\' \"></ng-include></tab>\n        <tab heading=\"History\" class=\"hidden-xs\" active=\"set.showHistory\"><ng-include src=\" \'profile/templates/profile_history.html\' \"></ng-include></tab>\n        <li role=\"tab\" data-toggle=\"tab\" class=\"dropdown hidden-md hidden-lg hidden-sm dropdown-toggle\" data-toggle=\"dropdown\">\n          <a href=\"#\">More <span class=\"caret\"></span></a>\n          <ul class=\"dropdown-menu\" role=\"menu\">\n            <li><a href=\"#\" ng-click=\"set.showHousehold = true\">Household</a></li>\n            <li><a href=\"#\" ng-click=\"set.showSkills = true\">Skills</a></li>\n            <li><a href=\"#\" ng-click=\"set.showHistory = true\">History</a></li>\n          </ul>\n        </li>\n      </tabset>\n    </div>\n  </div>\n");
$templateCache.put("profile/templates/profile_account.html","<form role=\"form\">\n\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"password\">Change Password</label>\n      <div class=\"input-group\">\n        <input type=\"password\" class=\"form-control\" id=\"password\" name=\"password\" ng-model=\"fields.password\" ng-if=\"!showPassword\" value=\"testpassword\" />\n        <input type=\"text\" class=\"form-control\" id=\"password-clear\" name=\"password\" ng-model=\"fields.password\" ng-if=\"showPassword\" value=\"testpassword\"/>\n        <span class=\"input-group-addon\">\n          <label>\n            <input type=\"checkbox\" ng-click=\"showPassword = !showPassword\"> Show\n          </label>\n        </span>\n      </div><!-- /input-group -->\n    </div>\n    <div class=\"form-group col-sm-12\">\n      <label class=\"block\">Email Communication</label>\n      <label class=\"radio-inline\">\n        <input type=\"radio\" name=\"subscription\" value=\"on\" checked=\"checked\"> On\n      </label>\n      <label class=\"radio-inline\">\n        <input type=\"radio\" name=\"subscription\" value=\"off\"> Off\n      </label>\n    </div>\n\n  </div>\n  <button class=\"btn btn-primary\" data-style=\"expand-left\">\n    Save\n  </button>\n</form>\n");
$templateCache.put("profile/templates/profile_giving.html","<h2>Giving History</h2>\n<h3 class=\"subheading\">Your history of financial giving to crossroads.</h3>\n<br>\n\n<p><strong>Statements Sent To:</strong></p>\n<address>\n  {{address.addressLine1}}<br>\n  <span ng-show=\"address.addressLine2\">{{address.addressLine2}}<br></span>\n  {{address.city}} {{address.state__Region}}, {{address.postalCode}}<br>\n</address>\n<br>\n<div ng-init=\"giving=[]; giving.total = 0\">\n<form class=\"form-inline\" role=\"form\">\n  <div class=\"form-group\">\n    <label for=\"dateFilter\" class=\"control-label\">Year</label>\n    <select class=\"form-control\" id=\"dateFilter\" ng-change=\"giving.total = 0\" ng-model=\"dateFilter\" ng-options=\"year for year in years\">\n      <option value=\"\">All</option>\n    </select>\n  </div>\n</form>\n<br>\n<table class=\"table table-striped table-responsive\">\n  <thead>\n    <tr>\n      <th>Date</th>\n      <th>Amount</th>\n      <th>Fund</th>\n      <th>Type</th>\n    </tr>\n  </thead>\n  <tr ng-repeat=\"givingRecord in givingHistory | DateYearFilter:dateFilter:\'donationDate\'\">\n    <td>{{givingRecord.donationDate | date:\"MM/dd/yyyy\"}}</td>\n    <td ng-init=\"giving.total = giving.total + givingRecord.amount\">{{givingRecord.amount | currency:\"$\"}}</td>\n    <td>{{givingRecord.statementTitle}}</td>\n    <td>{{givingRecord.paymentType}}</td>\n  </tr>\n</table>\n<h4>Total: {{giving.total | currency:\"$\"}}</h4>\n<br>\n</div>\n");
$templateCache.put("profile/templates/profile_history.html","<div class=\"history-timeline row\">\n  <ul class=\"timeline\">\n    <li>\n      <div class=\"timeline-badge default\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-users\">\n            <use xlink:href=\"/icons/cr.svg#users\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Attended Listening Training</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">7/10/2014</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge info\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-users\">\n            <use xlink:href=\"/icons/cr.svg#users\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Joined the India Interest Group</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">1/18/2014</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge info\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-users\">\n            <use xlink:href=\"/icons/cr.svg#users\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Joined the Fathers Oakley Fall 2011 Group</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">11/8/2013</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge default\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-calendar\">\n            <use xlink:href=\"/icons/cr.svg#calendar\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Attended Go Cincinnati</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">5/10/2013</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge info\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-users\">\n            <use xlink:href=\"/icons/cr.svg#users\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Joined the Crossroads.net Development Group</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">11/8/2012</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge info\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-users\">\n            <use xlink:href=\"/icons/cr.svg#users\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Joined the Crossroads.net Contractors Group</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">11/8/2012</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge success\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-gift\">\n            <use xlink:href=\"/icons/cr.svg#gift\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">My first financial gift</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">11/23/2003</p></div>\n        </div>\n      </div>\n    </li>\n    <li>\n      <div class=\"timeline-badge primary\">\n        <svg viewBox=\"0 0 32 32\" class=\"icon icon-large icon-close\">\n          <use xlink:href=\"/icons/cr.svg#close\"></use>\n        </svg>\n      </div>\n      <div class=\"timeline-panel\">\n        <div class=\"timeline-heading\">\n          <h5 class=\"timeline-title\">Attended my first Crossroads service</h5>\n          <div class=\"timeline-body\"><p class=\"text-muted\">7/14/2003</p></div>\n        </div>\n      </div>\n    </li>\n  </ul>\n</div>\n");
$templateCache.put("profile/templates/profile_household.html","<form role=\"form\" ng-controller=\"crdsProfileHousehold\">\n  <fieldset class=\"row household-cards\">\n    <legend class=\"col-sm-12\">Your Household</legend>\n    <div ng-repeat=\"member in family\">\n      <div class=\"col-md-4 profile-card clearfix\">\n        <div class=\"well well-sm\">\n          <div class=\"media\">\n            <div class=\"thumbnail pull-left\">\n              <img class=\"media-object\" src=\"//crossroads-media.s3.amazonaws.com/images/avatar.svg\" alt=\"Profile Default Photo\" />\n            </div>\n            <div class=\"media-body\">\n              <h4 class=\"media-heading\">{{member.displayName}}</h4>\n              <p><span class=\"label label-info\">{{member.householdPosition}}</span></p>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </fieldset>\n  <fieldset class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"street\">Street</label>\n      <input ng-model=\"address.addressLine1\" type=\"text\" class=\"form-control\" id=\"street\" placeholder=\"Enter street address\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"street2\">Street 2</label>\n      <input ng-model=\"address.addressLine2\" type=\"text\" class=\"form-control\" id=\"street2\" placeholder=\"Enter street address (cont.)\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"city\">City</label>\n      <input ng-model=\"address.city\" type=\"text\" class=\"form-control\" id=\"city\" placeholder=\"Enter city\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"state\">State</label>\n      <select  ng-model=\"address.state__Region\" ng-options=\"state.stateAbbreviation as state.stateAbbreviation for state in states\" class=\"form-control col-sm-12\">\n      </select>\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"zip\">Zip</label>\n      <input ng-model=\"address.postalCode\" type=\"text\" class=\"form-control\" id=\"zip\" placeholder=\"Enter zip code\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"county\">County</label>\n      <input ng-model=\"address.county\" type=\"text\" class=\"form-control\" id=\"county\" placeholder=\"Enter county\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"country\">Country</label>\n      <select ng-model=\"countryObj\" ng-change=\"setCountry()\" ng-options=\"country as country.dpRecordname for country in countries track by country.dpRecordname\" class=\"form-control col-sm-12\">\n      </select>\n    </div>\n\n    <hr class=\"col-sm-12\" />\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"homephone\">Home Phone</label>\n      <input ng-model=\"household.homePhone\" type=\"text\" class=\"form-control\" id=\"homephone\" placeholder=\"Enter home phone\">\n    </div>\n\n    <hr class=\"col-sm-12\" />\n    <div id=\"Site\" class=\"form-group col-sm-6\">\n      <label for=\"provider\">Crossroads Location</label>\n      <select class=\"form-control col-sm-12\" ng-model=\"household.congregationId\" ng-options=\"congregation.dpRecordid as congregation.congregationName for congregation in congregations\">\n      </select>\n    </div>\n  </fieldset>\n  <button id=\"save-household\" ng-click=\"saveHousehold()\" class=\"btn btn-primary\" data-style=\"expand-left\">\n    Save\n  </button>\n</form>\n");
$templateCache.put("profile/templates/profile_personal.html","<form role=\"form\" ng-controller=\"crdsProfilePersonal\">\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"firstname\">Email</label>\n      <input type=\"email\" class=\"form-control\" id=\"email\" name=\"email\" placeholder=\"Enter email address\" ng-model=\"contact.email\">\n    </div>\n  </div>\n\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"firstname\">First Name</label>\n      <input type=\"text\" class=\"form-control\" id=\"firstname\" placeholder=\"Enter first name\" ng-model=\"contact.first_name\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"middlename\">Middle Name</label>\n      <input type=\"text\" class=\"form-control\" id=\"middlename\" placeholder=\"Enter middle name\" ng-model=\"contact.middle_name\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"lastname\">Last Name</label>\n      <input type=\"text\" class=\"form-control\" id=\"lastname\" placeholder=\"Enter last name\" ng-model=\"contact.last_name\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"maidenname\">Maiden Name</label>\n      <input type=\"text\" class=\"form-control\" id=\"maidenname\" placeholder=\"Enter maiden name\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"nickname\">Nick Name <small class=\"text-muted\">(Example: Rob or Becky)</small></label>\n      <input type=\"text\" class=\"form-control\" id=\"nickname\" placeholder=\"Enter nick name\" ng-model=\"contact.nick_nname\">\n    </div>\n  </div>\n\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"nickname\">Mobile Phone</label>\n      <input type=\"text\" class=\"form-control\" id=\"mobile\" placeholder=\"Enter mobile phone\" ng-model=\"contact.mobile_phone\">\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label for=\"provider\">Service Provider</label>\n      <select class=\"form-control col-sm-12\">\n        {% for i in (1..31) %}\n          <option>{{ i }}</option>\n        {% endfor %}\n      </select>\n    </div>\n\n  </div>\n\n  <div class=\"row\">\n    <div class=\"col-sm-6\" ng-controller=\"crdsDatePickerCtrl\">\n      <label class=\"control-label\">Birth Date</label>\n      <p class=\"input-group\">\n        <input type=\"text\" class=\"form-control\" datepicker-popup=\"MM/dd/yyyy\" datepicker-append-to-body=\"true\" ng-model=\"contact.birth_date\" is-open=\"opened\" datepicker-options=\"dateOptions\" date-disabled=\"disabled(date, mode)\" ng-required=\"true\" close-text=\"Close\" ng-click=\"open($event)\" />\n        <span class=\"input-group-btn\">\n          <button type=\"button\" class=\"btn btn-primary\" ng-click=\"open($event)\"><svg viewBox=\"0 0 32 32\" class=\"icon icon-calendar\"><use xlink:href=\"/icons/cr.svg#calendar\"></use></svg></button>\n        </span>\n      </p>\n    </div>\n  </div>\n\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"marital_status\">Maritial Status</label>\n      <select class=\"form-control col-sm-12\"\n              ng-model=\"contact.maritalStatusId\"\n              ng-options=\"status.dpRecordid as status.dpRecordname for status in maritalStatuses\">\n      </select>\n    </div>\n\n    <div class=\"form-group col-sm-6\">\n      <label class=\"block\">Gender</label>\n      <label class=\"radio-inline\" ng-repeat=\"gender in genders\">\n        <input type=\"radio\" id=\"gender\" ng-value=\"gender.dpRecordid\" ng-model=\"contact.genderId\"> {{ gender.dpRecordname }}\n      </label>\n    </div>\n  </div>\n\n  <div class=\"row\">\n    <div class=\"form-group col-sm-6\">\n      <label for=\"employer\">Employer</label>\n      <input type=\"text\" class=\"form-control\" id=\"employer\" placeholder=\"Enter employer\" ng-model=\"contact.employer\">\n    </div>\n  </div>\n\n  <div class=\"row\">\n    <div class=\"col-sm-6\" ng-controller=\"crdsDatePickerCtrl\">\n      <label class=\"control-label\">When did you start attending Crossroads?</label></span>\n      <p class=\"input-group\">\n        <input type=\"text\" class=\"form-control\" datepicker-popup=\"MM/dd/yyyy\" ng-model=\"contact.crossroads_start_date\" is-open=\"opened\" datepicker-options=\"dateOptions\" date-disabled=\"disabled(date, mode)\" ng-required=\"true\" close-text=\"Close\" ng-click=\"open($event)\" />\n        <span class=\"input-group-btn\">\n          <button type=\"button\" class=\"btn btn-primary\" ng-click=\"open($event)\"><svg viewBox=\"0 0 32 32\" class=\"icon icon-calendar\"><use xlink:href=\"/icons/cr.svg#calendar\"></use></svg></button>\n        </span>\n      </p>\n    </div>\n  </div>\n\n  <button id=\"save-personal\" ng-click=\"savePersonal(contact)\" class=\"btn btn-primary\" data-style=\"expand-left\">\n    Save\n  </button>\n</form>\n");
$templateCache.put("profile/templates/profile_skills.html","<div >skills\n</div>\n");}]);

//# sourceMappingURL=app.js.map