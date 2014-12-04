crds-ng-profile
===============

AngularJS module for Ministry Platform profile view and edit

## Bower Install

    bower install crds-ng-profile

## How to Use
### Include the module

    angular.module("app", ["crds-ng-profile"])
    
### Use the directive 
Include the Profile using the minimal directive.  The profile content will be hidden if the user is logged out.   

    <crds-profile/>
         
Optionally reference a CSS class that could show a loading icon or text.
The loading class will be removed once the server has responded that the user is either logged in or logged out. 

    <crds-profile loading-class="loading"/>
    
Use the child directive to include a custom Login form when the user is logged out.  The login form will be hidden upon login and the profile content shown.

     <crds-profile loading-class="loading">
         <crds-profile-login>
             <div class="crds-login">
                 <!-- Login goes here -->
                 This is the login form
             </div>
         </crds-profile-login>
     </crds-profile>
     
         
Both directives will work as Attributes so this is equivalent to the former:
         
     <div crds-profile loading-class="loading">
         <div crds-profile-login>
             <div class="crds-login">
                 <!-- Login goes here 
                 This is the login form
             </div>
         </div>
     </div>
         
## Future Direction
Features to consider for future development

* Attribute to support a Login Redirect URL instead of a nested Login Form
         
## How to build and test locally
### Install Build Dependencies
    npm install
    bower install

### Execute the buildfile and watch tasks
    gulp 
    
### View test page at
    http://localhost:3000
    
### Build the distribution
    gulp build
    
Minified and Debug versions copied to:

* dist/crds_ng_profile.js
* dist/crds_ng_profile.min.js


