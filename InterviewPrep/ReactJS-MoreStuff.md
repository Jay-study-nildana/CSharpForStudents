# ReactJS Interview Reference Guide for Developers

---

## Table of Contents

1. [What is ReactJS?](#what-is-reactjs)
2. [ReactJS Architecture](#reactjs-architecture)
3. [Core Concepts & Terminology](#core-concepts--terminology)
4. [React Project Structure](#react-project-structure)
5. [Creating a Simple React App](#creating-a-simple-react-app)
6. [JSX Explained](#jsx-explained)
7. [Components: Functional vs Class](#components-functional-vs-class)
8. [Props and State](#props-and-state)
9. [Lifecycle Methods](#lifecycle-methods)
10. [Hooks](#hooks)
11. [Event Handling](#event-handling)
12. [Conditional Rendering](#conditional-rendering)
13. [Lists and Keys](#lists-and-keys)
14. [Forms and Controlled Components](#forms-and-controlled-components)
15. [Lifting State Up](#lifting-state-up)
16. [Context API](#context-api)
17. [Error Boundaries](#error-boundaries)
18. [React Router](#react-router)
19. [State Management (Redux, Zustand, etc.)](#state-management-redux-zustand-etc)
20. [Fetching Data (AJAX, Fetch, Axios)](#fetching-data-ajax-fetch-axios)
21. [Testing in React](#testing-in-react)
22. [Performance Optimization](#performance-optimization)
23. [Best Practices](#best-practices)
24. [Common ReactJS Interview Questions & Answers](#common-reactjs-interview-questions--answers)
25. [Advanced ReactJS Interview Questions & Answers](#advanced-reactjs-interview-questions--answers)
26. [Resources & Further Reading](#resources--further-reading)

---

## 1. What is ReactJS?

**ReactJS** is an open-source JavaScript library developed by Facebook for building user interfaces, especially single-page applications (SPAs).  
- Released in 2013
- Declarative, component-based approach
- Virtual DOM for efficient updates
- Popular for web apps, mobile apps (React Native), and more

---

## 2. ReactJS Architecture

- **View Layer Only:** Focuses on rendering UI, not on routing or state management.
- **Component-Based:** Everything is a reusable component.
- **Unidirectional Data Flow:** Parent passes data to child via props.

Diagram:
```
[App]
  ├─ [Header]
  ├─ [Main]
  │    ├─ [Sidebar]
  │    └─ [Content]
  └─ [Footer]
```

---

## 3. Core Concepts & Terminology

| Concept           | Description                                                         |
|-------------------|---------------------------------------------------------------------|
| **Component**     | Reusable piece of UI (function or class)                            |
| **JSX**           | JavaScript XML syntax for React components                          |
| **Props**         | Data passed from parent to child component                          |
| **State**         | Local data storage within a component                               |
| **Hook**          | Function for using state/lifecycle in functional components         |
| **Context**       | Provides global data accessible to components                       |
| **Virtual DOM**   | Lightweight copy of the real DOM for efficient updates              |
| **Key**           | Unique identifier for list items                                    |

---

## 4. React Project Structure

```
/my-react-app
  /public
    index.html
  /src
    /components
    /pages
    App.js
    index.js
    styles.css
  package.json
  README.md
```
- `index.js`: Entry point, renders `<App />`
- `App.js`: Root component, app structure

---

## 5. Creating a Simple React App

**index.js**
```javascript
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);
```

**App.js**
```javascript
import React from 'react';
function App() {
  return (
    <div>
      <h1>Hello, ReactJS!</h1>
    </div>
  );
}
export default App;
```

---

## 6. JSX Explained

- JSX is a syntax extension that allows mixing HTML in JavaScript.
- Transpiled to `React.createElement`.
- Must return a single root element.
- Attributes use camelCase: `className`, `onClick`.

Example:
```javascript
const element = <h1 className="title">Hello, World!</h1>;
```

---

## 7. Components: Functional vs Class

**Functional Component:**
```javascript
function Welcome(props) {
  return <h1>Hello, {props.name}</h1>;
}
```

**Class Component:**
```javascript
import React, { Component } from 'react';
class Welcome extends Component {
  render() {
    return <h1>Hello, {this.props.name}</h1>;
  }
}
```
- Functional: Recommended, supports hooks
- Class: Legacy, supports lifecycle methods

---

## 8. Props and State

**Props:**
- Read-only data passed from parent to child.
- Example:
  ```javascript
  <Greeting name="Jay" />
  ```

**State:**
- Mutable data managed by a component.
- Example with hooks:
  ```javascript
  const [count, setCount] = useState(0);
  ```

---

## 9. Lifecycle Methods

**Class Components:**
- `componentDidMount`
- `componentDidUpdate`
- `componentWillUnmount`

Example:
```javascript
componentDidMount() {
  // Called after component renders
}
```

**Functional Components:** Use hooks like `useEffect`.

---

## 10. Hooks

- Added in React 16.8 (2019)
- Use state/lifecycle in functional components

| Hook        | Description                                   |
|-------------|-----------------------------------------------|
| `useState`  | Local state                                   |
| `useEffect` | Side effects (data fetch, subscriptions, etc) |
| `useContext`| Access context                                |
| `useRef`    | Mutable value, access DOM                     |
| `useReducer`| Alternative to useState for complex state     |
| `useMemo`   | Memoize expensive calculations                |
| `useCallback`| Memoize functions                            |

Example:
```javascript
import React, { useState, useEffect } from 'react';
function Counter() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    document.title = `Count: ${count}`;
  }, [count]);

  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}
```

---

## 11. Event Handling

- Use camelCase: `onClick`, `onChange`
- Pass functions, not strings

Example:
```javascript
function Clicker() {
  function handleClick() {
    alert("Button clicked!");
  }
  return <button onClick={handleClick}>Click Me</button>;
}
```

---

## 12. Conditional Rendering

- Use JS expressions (`if`, `ternary`, `&&`)
```javascript
{isLoggedIn ? <LogoutButton /> : <LoginButton />}
{messages.length > 0 && <MessageList messages={messages} />}
```

---

## 13. Lists and Keys

- Render lists with `map`
- Use unique `key` prop for each item

Example:
```javascript
const items = ['a', 'b', 'c'];
return (
  <ul>
    {items.map(item => <li key={item}>{item}</li>)}
  </ul>
);
```

---

## 14. Forms and Controlled Components

- Controlled component: value managed by React state
- Example:
```javascript
function NameForm() {
  const [name, setName] = useState('');
  function handleChange(e) {
    setName(e.target.value);
  }
  function handleSubmit(e) {
    e.preventDefault();
    alert(`Submitted: ${name}`);
  }
  return (
    <form onSubmit={handleSubmit}>
      <input value={name} onChange={handleChange} />
      <button type="submit">Submit</button>
    </form>
  );
}
```

---

## 15. Lifting State Up

- Move state to the closest common ancestor to share between components.

Example:
```javascript
function Parent() {
  const [value, setValue] = useState('');
  return (
    <>
      <Input value={value} setValue={setValue} />
      <Display value={value} />
    </>
  );
}
```

---

## 16. Context API

- Share state globally without prop drilling

Create context:
```javascript
const ThemeContext = React.createContext('light');

function App() {
  return (
    <ThemeContext.Provider value="dark">
      <Toolbar />
    </ThemeContext.Provider>
  );
}

function Toolbar() {
  return <ThemeButton />;
}

function ThemeButton() {
  const theme = useContext(ThemeContext);
  return <button className={theme}>Theme Button</button>;
}
```

---

## 17. Error Boundaries

- Catch rendering errors in child components
- Only available in class components

Example:
```javascript
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }
  static getDerivedStateFromError(error) {
    return { hasError: true };
  }
  componentDidCatch(error, info) {
    // Log error
  }
  render() {
    if (this.state.hasError) {
      return <h1>Something went wrong.</h1>;
    }
    return this.props.children;
  }
}
```

---

## 18. React Router

- For client-side routing

Install: `npm install react-router-dom`
Example:
```javascript
import { BrowserRouter as Router, Route, Link, Switch } from 'react-router-dom';

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
      </nav>
      <Switch>
        <Route exact path="/" component={Home} />
        <Route path="/about" component={About} />
      </Switch>
    </Router>
  );
}
```

---

## 19. State Management (Redux, Zustand, etc.)

- For complex/global state, use libraries

**Redux Example:**
```javascript
import { createStore } from 'redux';
function counter(state = 0, action) {
  switch (action.type) {
    case 'INCREMENT': return state + 1;
    case 'DECREMENT': return state - 1;
    default: return state;
  }
}
const store = createStore(counter);
store.subscribe(() => console.log(store.getState()));
store.dispatch({ type: 'INCREMENT' });
```

- Other options: MobX, Zustand, Recoil

---

## 20. Fetching Data (AJAX, Fetch, Axios)

**Using Fetch:**
```javascript
useEffect(() => {
  fetch('https://api.example.com/data')
    .then(res => res.json())
    .then(data => setData(data));
}, []);
```

**Using Axios:**
```javascript
import axios from 'axios';
useEffect(() => {
  axios.get('/api/data').then(res => setData(res.data));
}, []);
```

---

## 21. Testing in React

- Use **Jest** (test runner) and **React Testing Library** (DOM testing)
- **Enzyme** (legacy, less recommended)

Example:
```javascript
import { render, screen, fireEvent } from '@testing-library/react';
import Counter from './Counter';

test('increments counter', () => {
  render(<Counter />);
  fireEvent.click(screen.getByText('Increment'));
  expect(screen.getByText('Count: 1')).toBeInTheDocument();
});
```

---

## 22. Performance Optimization

- Use `React.memo` for pure components
- Use `useMemo`, `useCallback` to prevent unnecessary renders
- Code splitting: `React.lazy` and `Suspense`
- Avoid unnecessary state updates
- Use keys for lists

---

## 23. Best Practices

- Break UI into small, reusable components
- Keep state minimal and local where possible
- Use prop-types or TypeScript for type safety
- Use hooks for side effects/state
- Avoid direct DOM manipulation
- Write tests for components and logic
- Use context/Redux only when needed
- Organize files logically (by feature, not type)

---

## 24. Common ReactJS Interview Questions & Answers

**Q1: What is ReactJS and why is it popular?**  
> ReactJS is a declarative JavaScript library for building UIs. It’s popular due to its component-based architecture, efficient virtual DOM updates, and strong ecosystem.

**Q2: What is JSX?**  
> JSX stands for JavaScript XML. It’s a syntax extension that allows writing HTML-like code in JS, making UI code more readable and maintainable.

**Q3: What is the virtual DOM and how does React use it?**  
> The virtual DOM is a lightweight copy of the actual DOM. React updates the virtual DOM first, then efficiently syncs changes with the real DOM, improving performance.

**Q4: What are props in React?**  
> Props are read-only inputs passed from parent to child components, used for data transfer and configuration.

**Q5: How is state different from props?**  
> State is local and mutable within a component; props are external and controlled by the parent.

**Q6: What are hooks in React?**  
> Hooks are functions that let you use state and lifecycle features in functional components (e.g., `useState`, `useEffect`).

**Q7: Explain useEffect and its dependencies.**  
> `useEffect` runs side effects after rendering. Dependencies determine when the effect runs; if empty, runs once; if array, runs when any listed value changes.

**Q8: How do you handle events in React?**  
> Use camelCase event props (`onClick`) and pass functions. Event handlers receive synthetic event objects.

**Q9: How do you update state in React?**  
> Use the state setter function (`setState` or `setCount`). Never modify state directly.

**Q10: What is conditional rendering?**  
> Render different UI based on state/props using JS expressions (ternary, `if`, `&&`).

**Q11: What are controlled and uncontrolled components?**  
> Controlled: Form value managed by React state. Uncontrolled: Form value managed by DOM.

**Q12: What is React Context?**  
> Context provides a way to share global data (e.g., theme, user) across components without prop drilling.

**Q13: What is React Router?**  
> Library for client-side routing, enabling multiple pages/views within a SPA.

**Q14: Why use Redux and when should you avoid it?**  
> Redux helps manage complex, global state. Avoid it for simple apps; prefer hooks/context for local state.

**Q15: How do you optimize React performance?**  
> Use memoization (`React.memo`, `useMemo`), code splitting, avoid unnecessary renders, use keys for lists.

---

## 25. Advanced ReactJS Interview Questions & Answers

**Q16: What is reconciliation in React?**  
> Reconciliation is the process React uses to update the DOM by comparing the new virtual DOM with the previous one and making minimal changes.

**Q17: Explain useRef and practical use cases.**  
> `useRef` creates a mutable object that persists across renders. Use for accessing DOM elements, storing timers, or tracking previous values.

**Q18: What is higher-order component (HOC)?**  
> An HOC is a function that takes a component and returns a new component, adding extra functionality.

**Q19: What is render props?**  
> Render props is a pattern where a component’s prop is a function that returns JSX, allowing dynamic rendering.

**Q20: How does code splitting work in React?**  
> Code splitting breaks app code into chunks loaded on demand using `React.lazy`, `Suspense`, or dynamic imports.

**Q21: What is prop drilling and how do you avoid it?**  
> Prop drilling is passing data through many component layers. Avoid with Context API or state management libraries.

**Q22: How do you handle errors in React?**  
> Use error boundaries (class components) to catch rendering errors; use try/catch in event handlers and async functions.

**Q23: Describe useReducer and when to use it.**  
> `useReducer` manages complex state logic, especially with multiple related state variables.

**Q24: How can you test React components?**  
> Use Jest for unit testing and React Testing Library for DOM interaction, checking rendered output and user events.

**Q25: What are pure components and why use them?**  
> Pure components only re-render when props/state change, improving performance by avoiding unnecessary renders.

**Q26: How do you manage side effects in React?**  
> Use `useEffect` for data fetching, subscriptions, timers, and clean up in the return function.

**Q27: What are portals in React?**  
> Portals render children into a DOM node outside the parent hierarchy, useful for modals or overlays.

**Q28: What is SSR and hydration in React?**  
> Server-side rendering (SSR) renders React on the server for faster initial load; hydration attaches React events to server-rendered markup.

**Q29: How do you implement lazy loading in React?**  
> Use `React.lazy` and `Suspense` to load components on demand, reducing initial bundle size.

**Q30: How does React work with TypeScript?**  
> React supports TypeScript for type safety. Use `.tsx` files, type props and state, and leverage community types.

---

## 26. Resources & Further Reading

- [React Official Docs](https://react.dev/)
- [ReactJS GitHub](https://github.com/facebook/react)
- [React Patterns](https://reactpatterns.com/)
- [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/)
- [Redux Documentation](https://redux.js.org/)
- [React Router Docs](https://reactrouter.com/)
- [Awesome React (GitHub)](https://github.com/enaqx/awesome-react)
- [Jest Testing Framework](https://jestjs.io/)

---

**Practical Exercise:**

1. Create a new React app using Create React App or Vite.
2. Build a multi-component UI: header, main, sidebar, and footer.
3. Add routing with React Router for two pages.
4. Implement form handling (controlled component) and validation.
5. Fetch data from a public API and display in a list.
6. Lift state up and share between two child components.
7. Add global state with Context or Redux.
8. Write unit tests for a component using React Testing Library.
9. Optimize performance with memoization and lazy loading.
10. Build and deploy the app to GitHub Pages or Vercel.

---

*Prepared for first-time developer interview candidates. This guide covers conceptual, practical, and best-practice aspects of ReactJS development and interview preparation.*