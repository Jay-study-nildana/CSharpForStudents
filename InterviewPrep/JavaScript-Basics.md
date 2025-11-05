# JavaScript — Basics, Operators, Control Flow, Objects, Arrays, Functions  
Interview Reference Guide for Developers

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Quick Version History & Environment Notes](#quick-version-history--environment-notes)  
3. [Language Fundamentals & Types](#language-fundamentals--types)  
4. [Values, Types and Coercion](#values-types-and-coercion)  
5. [Operators Overview](#operators-overview)  
   - Arithmetic, assignment, comparison, logical, bitwise, unary, ternary, nullish coalescing, optional chaining, spread/rest  
6. [Control Flow & Statements](#control-flow--statements)  
   - if/else, switch, loops (for, while, do/while), for...of, for...in, labeled statements, break/continue, try/catch/finally  
7. [Scope, Hoisting, Temporal Dead Zone & Closures](#scope-hoisting-temporal-dead-zone--closures)  
   - var/let/const differences, function scope, block scope, TDZ, lexical scope, closures  
8. [Functions — Declarations, Expressions, Arrow Functions & Parameters](#functions---declarations-expressions-arrow-functions--parameters)  
   - call/apply/bind, default parameters, rest parameters, arguments object, generator functions, IIFEs, higher-order functions, callbacks  
9. [Objects: Creation, Prototypes & Inheritance](#objects-creation-prototypes--inheritance)  
   - Object literals, property descriptors, getters/setters, prototype chain, Object.create, prototypes vs classes, instanceof, hasOwnProperty  
10. [Classes & ES6+ Syntax](#classes--es6-syntax)  
    - class syntax, constructor, static methods, inheritance (extends/super), private fields (#), class fields, mixins  
11. [Arrays & Array Methods](#arrays--array-methods)  
    - creation, iteration, mutating vs non-mutating methods, map/filter/reduce, find, some/every, sort, splice, slice, flat/flatMap, performance tips  
12. [Destructuring, Spread & Rest Patterns](#destructuring-spread--rest-patterns)  
13. [Modules: ES Modules vs CommonJS](#modules-es-modules-vs-commonjs)  
14. [Asynchronous JavaScript: Promises, async/await & Event Loop](#asynchronous-javascript-promises-asyncawait--event-loop)  
15. [Errors & Exception Handling](#errors--exception-handling)  
16. [Browser APIs & DOM Basics (if targeting web)](#browser-apis--dom-basics-if-targeting-web)  
17. [Testing, Debugging & Tooling](#testing-debugging--tooling)  
18. [Performance Considerations & Common Pitfalls](#performance-considerations--common-pitfalls)  
19. [Best Practices & Style Guidelines](#best-practices--style-guidelines)  
20. [Comprehensive Q&A — Developer & Interview Questions (with answers)](#comprehensive-qa--developer--interview-questions-with-answers)  
21. [Practical Exercises & Mini Projects](#practical-exercises--mini-projects)  
22. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This guide covers core JavaScript language concepts and patterns most useful for interviews and practical development: primitive and reference types, operators, control flow, functions, objects, arrays, ES6+ features, async programming, debugging, and best practices. It aims to be a concise reference for knowledge review and coding tasks.

---

## 2. Quick Version History & Environment Notes

- JavaScript originally standardized as ECMAScript (ES). Major milestones:
  - ES5 (2009): strict mode, JSON, Object.defineProperty, `Array.prototype` methods improvements.
  - ES6 / ES2015 (2015): let/const, arrow functions, classes, template literals, modules, Promises, Map/Set, destructuring.
  - ES2016+ annual updates: async/await (ES2017), object spread, optional chaining, nullish coalescing, BigInt, private class fields, top-level await, etc.
- Runtime environments: browsers and Node.js. Feature availability depends on target runtime (use transpilation/polyfills as needed).

---

## 3. Language Fundamentals & Types

JavaScript types (ECMAScript spec):
- Primitive types: undefined, null, boolean, number (IEEE 754), bigint, string, symbol.
- Object type: plain objects, arrays, functions, dates, regexps, Map, Set, etc.

typeof operator:
```js
typeof 1 === 'number';
typeof 'a' === 'string';
typeof undefined === 'undefined';
typeof null === 'object'; // historical quirk
typeof Symbol() === 'symbol';
typeof 10n === 'bigint';
```

Note: null is a primitive but typeof returns "object" (legacy).

Truthy/falsy:
- Falsy: false, 0, -0, "", null, undefined, NaN
- Everything else is truthy (including "0", [], {}).

NaN:
- NaN is not equal to itself; test via Number.isNaN(value).

---

## 4. Values, Types and Coercion

Type coercion:
- JavaScript performs implicit conversion in many contexts: `==`, `+` (string concatenation), boolean contexts.
- Prefer `===` and `!==` to avoid surprising coercions.

Examples:
```js
0 == '0'; // true
0 === '0'; // false
'' + 1 + 0; // "10"
'' - 1 + 0; // -1 (numeric conversion)
'5' * '2' // 10
null == undefined // true
null === undefined // false
```

Numeric pitfalls:
- Floating point precision: 0.1 + 0.2 !== 0.3
- Use Number.isFinite, Number.isInteger, and BigInt for large integers.

Conversion helpers:
- String(x), Number(x), Boolean(x), parseInt/parseFloat, Object.prototype.toString.call(x) for reliable type tagging.

Symbols:
- Unique identifiers used as object property keys, not enumerable by default.

---

## 5. Operators Overview

Arithmetic:
- +, -, *, /, %, ** (exponentiation), unary +/-

String concatenation:
- `+` concatenates when one operand is string.

Assignment:
- `=`, compound `+=, -=, *=, /=` etc.

Comparison:
- `==` (loose), `===` (strict), `<`, `>`, `<=`, `>=`
- Compare semantics with mixed types follow spec coercion; prefer `===`.

Logical:
- `&&`, `||`, `!` — with short-circuit evaluation and truthy/falsy semantics.
- Logical operators return the actual operand value (not just boolean) which enables idioms:
  ```js
  const name = user.name || 'Guest';
  ```
- Nullish coalescing `??` treats `null` and `undefined` as nullish (useful to allow falsy zero/empty string):
  ```js
  const val = x ?? defaultValue;
  ```

Optional chaining:
- `obj?.prop`, `obj?.[expr]`, `func?.()` prevents TypeError for null/undefined.

Ternary:
- `condition ? exprIfTrue : exprIfFalse`.

Bitwise:
- `& | ^ << >> >>>` used for integer-like operations; be careful with sign and 32-bit conversion.

Unary:
- `typeof`, `void`, `delete` (deletes object property), `!`, unary `+` for numeric conversion, unary `-`.

Spread / Rest:
- Spread in arrays/objects: `...[items]`, object spread `{ ...obj }`.
- Rest in function parameters and destructuring: `...rest`.

Destructuring:
- Object and array destructuring with defaults:
  ```js
  const { a, b = 2 } = obj;
  const [x, y] = arr;
  ```

Optional: pipeline operator (staged) — not yet standard uniformly.

---

## 6. Control Flow & Statements

Conditional:
```js
if (cond) { ... } else if (...) { ... } else { ... }
switch (value) {
  case 'a': ...
    break;
  default: ...
}
```
- Prefer `switch` for multi-way decisions; include `break` to avoid fallthrough or use fallthrough intentionally with comment.

Loops:
- `for (let i=0; i<10; i++) {}` — basic
- `while`, `do/while`
- `for...of` — iterates over iterable values (arrays, strings, Maps)
- `for...in` — iterates enumerable property names (not recommended for arrays)
- `Array.prototype.forEach`, `.map`, `.filter` for functional iteration

Break & continue:
- Standard usage; `label:` supported for nested loop control but rarely needed.

Exception handling:
```js
try {
  // possibly throwing code
} catch (err) {
  // handle
} finally {
  // always runs
}
```
- `throw` can throw any value (prefer Error objects).

Return behavior:
- `return` exits function; in arrow expression bodies, `()=> expr` returns expr implicitly.

---

## 7. Scope, Hoisting, Temporal Dead Zone & Closures

Scope types:
- Global scope (per module or global object)
- Function scope (for `var` and function declarations)
- Block scope (for `let` and `const`)

Hoisting:
- Function declarations and `var` declarations are hoisted. `let` and `const` are hoisted but remain in Temporal Dead Zone (TDZ) until initialized.

Examples:
```js
console.log(a); // undefined (var hoisted)
var a = 5;

console.log(b); // ReferenceError (TDZ)
let b = 3;

foo(); // works (function declaration hoisted)
function foo(){ console.log('hi'); }
```

Closures:
- Functions capture lexical environment (variables in outer scopes), enabling encapsulation and data privacy.
Example closure:
```js
function makeCounter() {
  let n = 0;
  return function() { n += 1; return n; };
}
const c = makeCounter();
c(); // 1
```

Common pitfalls:
- Capturing loop vars with `var` (use `let` to create per-iteration binding).

---

## 8. Functions — Declarations, Expressions, Arrow Functions & Parameters

Function forms:
- Function declaration:
  ```js
  function add(a,b) { return a+b; }
  ```
- Function expression:
  ```js
  const add = function(a,b) { return a+b; };
  ```
- Arrow function:
  ```js
  const add = (a,b) => a+b;
  const nop = () => {};
  const square = x => x*x;
  ```
  - Arrow functions have lexical `this` and cannot be used as constructors.
  - Shorter syntax; be careful with `this`, `arguments`, and `new`.

Parameters:
- Default parameters:
  ```js
  function f(a=1, b=2) { }
  ```
- Rest parameters:
  ```js
  function sum(...nums) { return nums.reduce((s,x)=>s+x, 0); }
  ```
- `arguments` object exists in non-arrow functions (array-like).

call/apply/bind:
- `fn.call(thisArg, ...args)`, `fn.apply(thisArg, argsArray)`, `fn.bind(thisArg, ...args)` returns bound function.

Generator functions:
- `function* gen() { yield 1; yield 2; }` produce iterators; useful for lazy sequences.

IIFE (Immediately Invoked Function Expression):
```js
(function(){ /* isolated scope */ })();
(() => { /* arrow IIFE */ })();
```

Higher-order functions:
- Functions that take or return functions (map, filter, reduce, currying).

Example: debounce & throttle (common utilities in UIs).

---

## 9. Objects: Creation, Prototypes & Inheritance

Object creation:
- Literal:
  ```js
  const obj = { a:1, b() { return 2; }, 'k': 'v' };
  ```
- Constructor:
  ```js
  function Person(name) { this.name = name; }
  Person.prototype.greet = function(){ console.log(this.name); };
  const p = new Person('A');
  ```
- Object.create:
  ```js
  const proto = { speak() { console.log('hi'); } };
  const o = Object.create(proto);
  ```

Property descriptors:
- `Object.defineProperty(obj, 'x', { value:1, writable:false, enumerable:false, configurable:false })`
- `Object.getOwnPropertyDescriptor` / `Object.getOwnPropertyNames`, `Object.keys`, `Object.entries`

Getters & Setters:
```js
const o = {
  get fullName() { return `${this.first} ${this.last}`; },
  set fullName(v) { [this.first, this.last] = v.split(' '); }
};
```

Prototype chain & lookup:
- `obj.__proto__` (deprecated) and `Object.getPrototypeOf(obj)` traverse prototype chain.
- `hasOwnProperty` checks own properties vs prototype ones:
  ```js
  obj.hasOwnProperty('toString'); // likely false
  ```

in operator:
- `'x' in obj` checks property existence on object or prototype chain.

Enumerability:
- Default for object literal properties is enumerable.

Symbols as keys:
- `const s = Symbol('id'); obj[s] = 123;` not enumerated in for...in, but retrievable via `Object.getOwnPropertySymbols`.

---

## 10. Classes & ES6+ Syntax

Class syntax (syntactic sugar over prototype-based inheritance):
```js
class Animal {
  constructor(name) { this.name = name; }
  speak() { console.log(this.name); }
  static create(name) { return new Animal(name); }
}
class Dog extends Animal {
  constructor(name, breed) { super(name); this.breed = breed; }
  speak() { super.speak(); console.log('woof'); }
}
```

Fields & private fields:
- Public field (proposal supported in modern engines):
  ```js
  class C { state = 1; }
  ```
- Private fields with `#`:
  ```js
  class Counter {
    #count = 0;
    inc() { this.#count++; return this.#count; }
  }
  ```

Mixins and composition:
- Prefer composition over inheritance for complex cases.

`new.target`:
- Detect if a function was called with `new`.

---

## 11. Arrays & Array Methods

Array creation:
```js
const a = [1,2,3];
const b = Array.from('abc'); // ['a','b','c']
const c = Array.of(1,2,3);
```

Mutating methods (modify array):
- push, pop, shift, unshift, splice, sort, reverse, fill, copyWithin

Non-mutating methods (return new arrays or values):
- map, filter, reduce, slice, concat, flat, flatMap

Iteration:
- for, for...of, forEach, map/filter/reduce, entries(), keys(), values()

Useful examples:
```js
const doubled = arr.map(x => x * 2);
const evens = arr.filter(x => x % 2 === 0);
const sum = arr.reduce((s, x) => s + x, 0);
const flat = nested.flat(2);
```

Performance:
- Avoid frequent shifting/unshifting on large arrays (O(n)).
- Prefer typed arrays for numeric performance-critical use.

Arrays vs objects:
- Arrays are objects with numeric keys and length property; `Array.isArray` to test.

Sparse arrays:
- Holes vs explicit undefined — be cautious with iteration.

Set / Map:
- ES6 Map and Set for key/value and unique collection semantics:
  ```js
  const m = new Map([[k1,v1]]);
  const s = new Set([1,2,2]); // Set {1,2}
  ```

---

## 12. Destructuring, Spread & Rest Patterns

Destructuring:
```js
const [a, b] = [1,2];
const { x, y = 5 } = obj;
const { name: username } = person;
```

Spread:
- Arrays: `const copy = [...arr]; const merged = [...a, ...b];`
- Objects: `const merged = { ...obj1, ...obj2 };` (later properties overwrite earlier ones)

Rest:
- Function parameters: `function f(...args) { }`
- Destructuring rest: `const [head, ...tail] = list;` or `const { a, ...rest } = obj;`

Shallow copy warnings:
- Spread/assign do shallow copies only (nested objects are shared references).

---

## 13. Modules: ES Modules vs CommonJS

CommonJS (Node.js historically):
```js
// module.exports
module.exports = { foo };
const lib = require('./lib');
```

ES Modules (ESM):
```js
// export
export function f() {}
export default MyClass;
// import
import { f } from './lib.js';
import MyClass from './lib.js';
```

Differences:
- ESM supports static analysis, tree-shaking, top-level `import`/`export` and `import.meta`.
- `require` is synchronous; `import` is asynchronous (module specifiers may be resolved before script runs).
- In Node.js, use `"type": "module"` in package.json or `.mjs` extension for ESM.

Circular dependencies:
- Both systems can have circular dependencies; ESM provides live bindings (exports reflect runtime changes), CommonJS snapshots the exports object at require time.

Default export vs named exports:
- Prefer named exports for tooling and clarity; default exports useful for a single main export.

---

## 14. Asynchronous JavaScript: Promises, async/await & Event Loop

Promises:
```js
const p = new Promise((resolve, reject) => {
  // async work
});
p.then(result => {}).catch(err => {}).finally(() => {});
```

Promise combinators:
- Promise.all (fails fast, returns array), Promise.allSettled, Promise.race, Promise.any

async/await:
```js
async function fetchData() {
  try {
    const r = await fetch(url);
    const data = await r.json();
    return data;
  } catch (e) { throw e; }
}
```
- `await` waits for a promise; `async` functions return a promise.

Event loop & microtasks:
- Microtasks (Promise callbacks, queueMicrotask) run before next macrotask (setTimeout).
- Understanding event loop is crucial to reason about order: for example, `console.log(1); setTimeout(...); Promise.resolve().then(...);` will run promise callback before setTimeout.

Concurrency:
- JavaScript is single-threaded in the main event loop (except Web Workers / Worker threads).
- Use concurrency primitives (worker threads, Web Workers) for CPU-bound tasks.

Common patterns:
- Debouncing/throttling, request batching, backoff strategies for network retries.

---

## 15. Errors & Exception Handling

Error types:
- `Error`, `TypeError`, `ReferenceError`, `RangeError`, `SyntaxError`, `URIError`, `EvalError`
- Create custom errors:
  ```js
  class MyError extends Error {
    constructor(msg) { super(msg); this.name = 'MyError'; }
  }
  ```

Throwing:
- Throw Error objects for stack traces and consistent handling:
  ```js
  throw new Error('Bad things');
  ```

Async errors:
- In promises, unhandled rejections may be captured by `unhandledrejection` event in browsers or `process.on('unhandledRejection')` in Node.
- Use `try/catch` with `await`.

Error handling strategy:
- Fail fast on invariants; validate inputs early.
- Add contextual information when rethrowing (wrap or log with metadata).
- Avoid swallowing errors silently.

---

## 16. Browser APIs & DOM Basics (if targeting web)

DOM manipulation:
```js
const el = document.querySelector('#id');
el.textContent = 'Hi';
el.classList.add('active');
el.addEventListener('click', handler);
```

Event delegation:
- Attach event listener to parent and inspect `event.target` for performance on many child elements.

Fetch API:
```js
fetch('/api')
  .then(r => r.json())
  .then(data => console.log(data));
```
- Use `AbortController` to cancel requests.

Local storage & session storage:
- `localStorage.setItem('k', JSON.stringify(obj));`
- `sessionStorage` for session-bound storage.

Service Workers & PWA:
- Background sync, caching strategies, offline behavior — advanced topics.

Security:
- Avoid `eval`, sanitize user input before DOM injection, use CSP.

---

## 17. Testing, Debugging & Tooling

Debugging:
- Browser DevTools: breakpoints, watch expressions, call stack, step over/in/out.
- Node inspector: `node --inspect` and DevTools or VS Code debug integration.
- Console methods: `console.log`, `console.table`, `console.time`/`timeEnd`, `console.trace`, `console.group`.

Linters & formatters:
- ESLint for code quality and conventions.
- Prettier for code formatting.

Unit & integration testing:
- Jest, Mocha + Chai, Jasmine for unit tests.
- Testing Library for DOM testing.
- Playwright, Cypress for end-to-end.

Type checking:
- TypeScript for static typing, or Flow. JSDoc type annotations for plain JS.

Bundlers & build tools:
- Vite, Webpack, Rollup, Parcel.
- Transpilation via Babel for cross-browser compatibility.
- Use source maps for better debugging in production.

---

## 18. Performance Considerations & Common Pitfalls

Memory:
- Avoid memory leaks: references from DOM to closures or global caches prevent GC.
- Clear intervals/timeouts and remove event listeners on teardown.

Reflow & repaint:
- Batch DOM reads and writes; avoid layout thrashing.
- Use `requestAnimationFrame` for animation updates.

Avoid heavy synchronous work:
- Offload CPU-heavy tasks to Web Workers.

Inefficient patterns:
- `.innerHTML +=` in loops re-parses HTML; use DocumentFragment or build strings and assign once.
- `.forEach` vs `for` — similar performance but prefer simplest readable approach unless micro-optimization needed.

Large arrays:
- Avoid repeated `.shift()` on large arrays; prefer pointer/index or Deque.

Serialization:
- JSON.stringify on large objects is expensive; minimize frequency.

---

## 19. Best Practices & Style Guidelines

- Use `const` by default; `let` when reassignment needed; avoid `var`.
- Prefer pure functions and immutability when possible.
- Use descriptive variable and function names.
- Keep functions small and focused (single responsibility).
- Use Promises/async-await for async code; avoid callback hell.
- Fail fast and validate inputs.
- Document APIs with JSDoc or TypeScript types.
- Use feature detection (not user-agent sniffing) for progressive enhancement.
- Keep side effects explicit; avoid global state.

---

## 20. Comprehensive Q&A — Developer & Interview Questions (with answers)

Q1: What's the difference between `let`, `const`, and `var`?  
A: `var` is function-scoped and hoisted with initialization to undefined, allowing redeclaration. `let` and `const` are block-scoped and in TDZ until initialization; `const` cannot be reassigned (but objects can be mutated). Prefer `const` and `let`.

Q2: How does `this` behave in JavaScript?  
A: `this` is dynamic: in function calls default is global/undefined (strict), in method calls `this` is the receiver object, arrow functions capture lexical `this` from enclosing scope, `call/apply/bind` can set `this`.

Q3: What is hoisting?  
A: Declarations (function declarations and `var`) are moved to top of scope during compilation phase; `let`/`const` are hoisted but in TDZ.

Q4: Explain the event loop and microtasks vs macrotasks.  
A: Microtasks (Promise callbacks, queueMicrotask) run after the current call stack finishes and before the next macrotask (setTimeout, I/O). This affects ordering of async operations.

Q5: How does prototypal inheritance work?  
A: Objects have an internal prototype pointer to another object; property lookup traverses prototype chain; functions have `.prototype` used when created via `new`. `Object.create(proto)` creates an object with a custom prototype.

Q6: What's the difference between `==` and `===`?  
A: `==` performs type coercion before comparison; `===` compares without coercion. Prefer `===`.

Q7: Explain closures and a common pitfall with loops.  
A: Closure captures variables by reference; in loops with `var` the same variable is shared across iterations, leading to unexpected results. Use `let` (block-scoped) or capture loop variable in closure.

Q8: How do you handle errors in async/await?  
A: Wrap `await` in `try/catch`. For multiple parallel awaits use `Promise.allSettled` or `try/catch` per await as needed.

Q9: What's the difference between `null` and `undefined`?  
A: `undefined` means a variable/property hasn't been assigned; `null` is an explicit empty value. Coercion: `null == undefined` true, but `null === undefined` false.

Q10: When should you use object vs Map for key-value storage?  
A: Use Map for arbitrary keys (including objects), predictable iteration order, and performance for frequent additions/removals. Use plain object for simple JSON-like data and JSON serialization.

Q11: How can you create private data in objects?  
A: Use closures (factory function), ES private fields `#`, or WeakMap for per-instance private storage.

Q12: Differences between deep and shallow copy — how to clone an object?  
A: Shallow copy (`{ ...obj }`, `Object.assign`) copies top-level properties; deep copy requires recursion or structuredClone / JSON (JSON fails for functions, undefined, symbols). Use structuredClone in modern environments for deep cloning.

Q13: What is memoization? Give an example.  
A: Caching function results to avoid recomputation (e.g., Fibonacci dynamic programming). Example:
```js
function memoize(fn) {
  const cache = new Map();
  return function(...args) {
    const key = JSON.stringify(args);
    if (cache.has(key)) return cache.get(key);
    const res = fn(...args);
    cache.set(key, res);
    return res;
  }
}
```

Q14: Explain `Function.prototype.bind`.  
A: `bind` returns a new function with `this` permanently set to the provided value and optional leading arguments pre-filled.

Q15: How do you avoid callback hell?  
A: Use Promises and async/await, split into smaller functions, and use control flow libraries if needed.

---

## 21. Practical Exercises & Mini Projects

1. Basics:
   - Implement `sum(...nums)` using rest parameters and reduce.
   - Write a `range(start, end, step=1)` function returning an array.

2. Intermediate:
   - Create a simple Pub/Sub system (subscribe, unsubscribe, publish).
   - Implement `debounce(fn, wait)` and `throttle(fn, wait)` utilities.
   - Build a todo list app (vanilla JS) using localStorage, showing DOM manipulation and event handling.

3. Advanced:
   - Implement a small module bundler simulation: parse simple import/export strings and resolve dependencies.
   - Build a simple Promise-based task queue with concurrency limit.
   - Create a tiny reactive system: `reactive(obj)` and `effect(fn)` that re-runs when dependencies change (basic dependency tracking).

4. Testing:
   - Write unit tests for utilities using Jest or Mocha.
   - Add end-to-end test for todo app using Playwright/Cypress.

---

## 22. References & Further Reading

- ECMAScript Language Specification — https://tc39.es/ecma262/  
- MDN Web Docs — JavaScript — https://developer.mozilla.org/en-US/docs/Web/JavaScript  
- "You Don't Know JS" (Kyle Simpson) — deep dive into JavaScript internals  
- "JavaScript: The Good Parts" (Douglas Crockford) — classic perspective  
- Node.js documentation — https://nodejs.org/en/docs/  
- TC39 proposals and stage process — https://github.com/tc39/proposals  
- Browser DevTools and performance docs (Chrome DevTools, Firefox DevTools)

---

Prepared as a compact but thorough reference for JavaScript fundamentals, commonly asked interview topics, and practical coding exercises. Use this file as a study aid, quick reference during coding, or as a guide when preparing for technical interviews on JavaScript fundamentals and modern ES features.