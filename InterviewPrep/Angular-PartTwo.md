# AngularJS Interview Reference Guide for Developers

---

## Table of Contents

1. [What is AngularJS?](#what-is-angularjs)
2. [AngularJS Architecture](#angularjs-architecture)
3. [Core Concepts & Terminology](#core-concepts--terminology)
4. [AngularJS Project Structure](#angularjs-project-structure)
5. [Creating a Simple AngularJS App](#creating-a-simple-angularjs-app)
6. [Directives](#directives)
7. [Controllers and $scope](#controllers-and-scope)
8. [Modules and Dependency Injection](#modules-and-dependency-injection)
9. [Services & Factories](#services--factories)
10. [Filters](#filters)
11. [Routing](#routing)
12. [Forms & Validation](#forms--validation)
13. [Communication with Server (AJAX, $http, $resource)](#communication-with-server-ajax-http-resource)
14. [Testing in AngularJS](#testing-in-angularjs)
15. [Best Practices](#best-practices)
16. [Common AngularJS Interview Questions & Answers](#common-angularjs-interview-questions--answers)
17. [Advanced AngularJS Interview Questions & Answers](#advanced-angularjs-interview-questions--answers)
18. [Resources & Further Reading](#resources--further-reading)

---

## 1. What is AngularJS?

**AngularJS** is a JavaScript-based open-source front-end framework maintained by Google.  
- Released in 2010, supports building dynamic, single-page web applications (SPAs).
- Uses MV* architectural patterns (MVC, MVVM, etc.).
- Declarative templates, dependency injection, two-way data binding.

---

## 2. AngularJS Architecture

- **View:** HTML with AngularJS markup (directives, expressions).
- **Model:** Data managed via `$scope` (or components/services).
- **Controller:** JavaScript functions that manage application logic.
- **Services:** Singleton objects for shared code/data.
- **Directives:** Extend HTML with custom attributes/elements.
- **Dependency Injection:** Built-in for modular code.

---

## 3. Core Concepts & Terminology

| Concept           | Description                                                                  |
|-------------------|------------------------------------------------------------------------------|
| **Module**        | Container for the application (`angular.module`)                             |
| **Controller**    | JS function managing part of the app                                         |
| **$scope**        | Object that binds the controller and view                                    |
| **Directive**     | Custom HTML element/attribute for extending behavior                         |
| **Service**       | Singleton for reusable logic/data                                            |
| **Factory**       | Similar to service, but returns an object                                    |
| **Filter**        | Format data for display                                                      |
| **Expression**    | Code snippets in curly braces (`{{ }}`)                                      |
| **Two-way Data Binding** | Syncs model and view automatically                                    |

---

## 4. AngularJS Project Structure

```
/my-angularjs-app
  /app
    /controllers
    /services
    /directives
    /filters
    app.js
  index.html
  /lib
    /angular
  /css
  /images
```
- `index.html`: Main HTML file
- `app.js`: Main module, config, and bootstrap

---

## 5. Creating a Simple AngularJS App

**index.html**
```html
<!DOCTYPE html>
<html ng-app="myApp">
<head>
  <script src="lib/angular/angular.min.js"></script>
  <script src="app/app.js"></script>
</head>
<body>
  <div ng-controller="MainCtrl">
    <h1>{{ greeting }}</h1>
    <input ng-model="greeting">
  </div>
</body>
</html>
```

**app/app.js**
```javascript
angular.module('myApp', [])
  .controller('MainCtrl', function($scope) {
    $scope.greeting = 'Hello, AngularJS!';
  });
```

---

## 6. Directives

- Extend HTML with custom behavior.
- Built-in: `ng-model`, `ng-repeat`, `ng-if`, `ng-show`, `ng-hide`, etc.
- Custom directive example:
```javascript
angular.module('myApp').directive('myDirective', function() {
  return {
    restrict: 'E',
    template: '<div>Hello from directive!</div>'
  };
});
```
Usage:
```html
<my-directive></my-directive>
```

---

## 7. Controllers and $scope

- Controllers manage app logic.
- `$scope` links data/model to view.
- Example:
```javascript
angular.module('myApp').controller('UserCtrl', function($scope) {
  $scope.user = { name: 'Jane', age: 25 };
});
```
View:
```html
<div ng-controller="UserCtrl">
  <p>{{ user.name }}</p>
  <p>{{ user.age }}</p>
</div>
```

---

## 8. Modules and Dependency Injection

- Define modules:  
  `angular.module('myApp', ['ngRoute', 'ngAnimate']);`
- Inject dependencies into controllers, services, etc.:
```javascript
angular.module('myApp').controller('DemoCtrl', function($scope, $http) { /* ... */ });
```

---

## 9. Services & Factories

- **Service:** Returns instance; uses `this` keyword.
- **Factory:** Returns object or function.

Example Service:
```javascript
angular.module('myApp').service('MathService', function() {
  this.add = function(a, b) { return a + b; };
});
```
Example Factory:
```javascript
angular.module('myApp').factory('MathFactory', function() {
  return {
    add: function(a, b) { return a + b; }
  };
});
```

---

## 10. Filters

- Format data for display.
- Usage: `{{ value | uppercase }}`

Custom filter:
```javascript
angular.module('myApp').filter('reverse', function() {
  return function(input) {
    return input.split('').reverse().join('');
  };
});
```
Usage:
```html
<p>{{ 'AngularJS' | reverse }}</p> <!-- SJralugnA -->
```

---

## 11. Routing

- Use `ngRoute` or `ui-router` for SPAs.

Example with ngRoute:
```javascript
angular.module('myApp', ['ngRoute'])
  .config(function($routeProvider) {
    $routeProvider
      .when('/home', { templateUrl: 'home.html', controller: 'HomeCtrl' })
      .when('/about', { templateUrl: 'about.html', controller: 'AboutCtrl' })
      .otherwise({ redirectTo: '/home' });
  });
```
In index.html:
```html
<div ng-view></div>
```

---

## 12. Forms & Validation

- Bind form fields with `ng-model`.
- Built-in validation: `required`, `ng-minlength`, `ng-pattern`, etc.
- Example:
```html
<form name="userForm">
  <input type="text" ng-model="user.name" required>
  <span ng-show="userForm.$invalid">Name is required!</span>
</form>
```

---

## 13. Communication with Server (AJAX, $http, $resource)

- Use `$http` for AJAX:
```javascript
$http.get('/api/users').then(function(response) {
  $scope.users = response.data;
});
```
- Use `$resource` for RESTful calls (requires `ngResource`):
```javascript
var User = $resource('/api/users/:id');
User.get({ id: 1 }, function(user) { $scope.user = user; });
```

---

## 14. Testing in AngularJS

- Use **Jasmine** (unit testing) and **Karma** (test runner).
- Mock dependencies with **ngMock**.
- Example test:
```javascript
describe('MainCtrl', function() {
  beforeEach(module('myApp'));
  var $controller, $rootScope;
  beforeEach(inject(function(_$controller_, _$rootScope_) {
    $controller = _$controller_;
    $rootScope = _$rootScope_;
  }));
  it('should set greeting', function() {
    var $scope = $rootScope.$new();
    $controller('MainCtrl', { $scope: $scope });
    expect($scope.greeting).toBe('Hello, AngularJS!');
  });
});
```

---

## 15. Best Practices

- Use modules to organize code.
- Keep controllers thin; put logic in services.
- Use one-way data flow when possible.
- Avoid using `$scope` globally.
- Prefer components/directives for reusable UI.
- Use track by with `ng-repeat` for performance.
- Use dependency injection, avoid global variables.
- Write tests for controllers, services, and directives.
- Use constants and values for configuration.

---

## 16. Common AngularJS Interview Questions & Answers

**Q1: What is AngularJS and how does it differ from Angular (2+)?**  
> AngularJS is a JavaScript-based framework for building SPAs using MVC/MVVM. Angular (2+) is a TypeScript-based, component-driven framework with improved performance, modularity, and mobile support.

**Q2: What is two-way data binding?**  
> Changes in the view update the model, and changes in the model update the view automatically.

**Q3: What is a directive? Name some built-in directives.**  
> A directive adds behavior to HTML elements. Built-in examples: `ng-model`, `ng-repeat`, `ng-if`, `ng-show`, `ng-hide`.

**Q4: How do you communicate between controllers?**  
> Use services or events (`$emit`, `$broadcast`, `$on`) to share data or messages.

**Q5: What is $scope and how is it used?**  
> `$scope` is an object that links the view and controller, holding model data and functions.

**Q6: What are services and how do they differ from factories?**  
> Services are singletons created with the `new` keyword, factories return objects/functions. Both are used for reusable logic/data.

**Q7: How do you handle routes in AngularJS?**  
> Use ngRoute or ui-router to map URLs to templates and controllers.

**Q8: What is dependency injection and why is it important?**  
> DI automatically provides required dependencies to components, making code modular, testable, and easy to change.

**Q9: How do you handle form validation in AngularJS?**  
> Use built-in validators (`required`, `ng-minlength`, etc.), custom validators, and `$valid`/$invalid flags.

**Q10: How can you optimize performance in AngularJS apps?**  
> Limit watchers, use track by in ng-repeat, lazy load modules, minimize DOM manipulations, and use one-time bindings when possible.

---

## 17. Advanced AngularJS Interview Questions & Answers

**Q11: What is digest cycle in AngularJS?**  
> The digest cycle is the process AngularJS uses to check for changes in scope variables and update the DOM accordingly.

**Q12: How does $apply() work?**  
> `$apply()` triggers a digest cycle manually, useful when changes are made outside AngularJS (e.g., in event handlers).

**Q13: What is $watch and when would you use it?**  
> `$watch` observes changes to scope variables. Use it for custom logic when a variable changes.

**Q14: What are isolated scope directives and why are they important?**  
> Isolated scope allows directives to have their own scope, preventing unintended interactions with parent scopes.

**Q15: How do you create a custom filter?**  
> Use `angular.module().filter()`. Return a function that takes input and returns formatted output.

**Q16: What is $q and how is it used?**  
> `$q` is AngularJS’s promise/deferred API, used for asynchronous operations.

**Q17: What is transclusion in AngularJS directives?**  
> Transclusion allows directives to use custom HTML content inside their templates.

**Q18: How do you unit test an AngularJS controller?**  
> Use Jasmine/Karma, inject the controller and mock dependencies, then test behavior and scope.

**Q19: What is the role of $rootScope?**  
> `$rootScope` is the top-level scope, accessible everywhere. Use sparingly to avoid tight coupling.

**Q20: How can you migrate an AngularJS app to Angular (2+)?**  
> Use ngUpgrade for gradual migration, refactor code to components/services, and replace AngularJS modules with Angular ones.

---

## 18. Resources & Further Reading

- [AngularJS Official Docs](https://docs.angularjs.org/)
- [AngularJS Style Guide](https://github.com/johnpapa/angular-styleguide)
- [Egghead: AngularJS Tutorials](https://egghead.io/technologies/angularjs)
- [ng-newsletter](http://www.ng-newsletter.com/)
- [Karma Test Runner](https://karma-runner.github.io/)
- [Jasmine Testing Framework](https://jasmine.github.io/)
- [Awesome AngularJS (GitHub)](https://github.com/gianarb/awesome-angularjs)

---

**Practical Exercise:**

1. Create a new AngularJS app with two pages (home and about) using routing.
2. Add a form with validation and display error messages.
3. Create a custom directive to show a tooltip.
4. Use a service to fetch data from a mock API and display in a list.
5. Add unit tests for a controller and service using Jasmine and Karma.

---

*Prepared for first-time developer interview candidates. This guide covers conceptual, practical, and best-practice aspects of AngularJS development and interview preparation.*