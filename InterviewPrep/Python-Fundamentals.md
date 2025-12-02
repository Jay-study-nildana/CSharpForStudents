# Python — Fundamentals, Basics, Data Structures, Control Flow, Functions, Modules & Error Handling  
Practical Reference with Examples

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [General Overview & Introduction to Python](#general-overview--introduction-to-python)  
3. [History & Evolution of Python](#history--evolution-of-python)  
4. [Installing Python & Setting Up Environment](#installing-python--setting-up-environment)  
   - System installers, pyenv, venv, virtualenv, pipx, requirements.txt  
5. [Basics — REPL, Scripts, Running Python](#basics---repl-scripts-running-python)  
6. [Data Types & Variables](#data-types--variables)  
   - Numbers, Booleans, None, Strings, Bytes  
   - Mutability & identity vs equality  
7. [Common Built-in Collections](#common-built-in-collections)  
   - Lists, Tuples, Sets, Dictionaries — creation, common operations, comprehensions  
8. [Operators](#operators)  
   - Arithmetic, comparison, assignment, boolean, membership, identity, bitwise, augmented assignments  
9. [Strings — operations & formatting](#strings---operations--formatting)  
   - Slicing, methods, f-strings, bytes/encode-decode  
10. [Control Flow — Conditional Statements & Loops](#control-flow---conditional-statements--loops)  
    - if/elif/else, for, while, loop else, break/continue, enumerate, zip, iterators, generators  
11. [Functions — definition, arguments, scope & advanced topics](#functions---definition-arguments-scope--advanced-topics)  
    - Positional/default/keyword-only/varargs/kwargs, annotations, return, closures, `nonlocal`/`global`, lambdas, partials, decorators, generator functions  
12. [Modules & Packages — Importing, Creating, and Best Practices](#modules--packages---importing-creating-and-best-practices)  
    - sys.path, `__main__`, `__init__.py`, package layout, relative imports, pip install, standard library highlights  
13. [Error Handling & Debugging Techniques](#error-handling--debugging-techniques)  
    - try/except/finally/else, common exceptions, raising exceptions, custom exceptions, logging, pdb, tracebacks, assertions, typing and static checking (mypy)  
14. [Practical Examples & Small Recipes](#practical-examples--small-recipes)  
    - CSV reading, simple HTTP request, list dedupe, flatten nested lists, memoization  
15. [Best Practices & Style Guidelines](#best-practices--style-guidelines)  
16. [Further Reading & References](#further-reading--references)

---

## 1. Scope & Purpose

This guide is a compact but practical reference to Python fundamentals aimed at beginners and intermediate developers. It includes installation notes, basic language constructs, common data structures, functions, modules, and error handling with runnable code snippets.

Target Python versions: Python 3.8+ (recommend 3.11/3.12 for performance and modern features).

---

## 2. General Overview & Introduction to Python

Python is a high-level, interpreted, dynamically typed programming language emphasizing readability and productivity. It is used for web development, data science, automation, scripting, systems programming, and more.

Key features:
- Readable syntax (significant indentation)
- Batteries-included standard library
- Strong community and ecosystem (PyPI)
- Multi-paradigm: procedural, OOP, functional

---

## 3. History & Evolution of Python

- Created by Guido van Rossum, first released in 1991.
- Major milestones:
  - Python 2.x (legacy) — end-of-life in 2020.
  - Python 3.0 (2008) — breaking changes and long-term improvements.
  - Ongoing annual/regular releases (3.7, 3.8, 3.9, 3.10, 3.11, 3.12) adding typing, performance, syntax improvements (match/case, pattern matching).
- PEPs (Python Enhancement Proposals) provide the language evolution path.

---

## 4. Installing Python & Setting Up Environment

System installers:
- Windows: download from python.org or use Microsoft Store. Add to PATH checkbox.
- macOS: use Homebrew (`brew install python`) or official installer.
- Linux: use distro packages (`apt`, `dnf`, `pacman`) or pyenv for multiple versions.

pyenv (manage multiple versions)
```bash
# macOS / Linux
curl https://pyenv.run | bash
# then install a version
pyenv install 3.11.4
pyenv global 3.11.4
```

Recommended project environment (venv)
```bash
# create and activate a venv (Unix/macOS)
python -m venv .venv
source .venv/bin/activate

# Windows PowerShell
python -m venv .venv
.venv\Scripts\Activate.ps1
```

Install packages with pip (inside venv)
```bash
pip install requests flask
pip freeze > requirements.txt
pip install -r requirements.txt
```

pipx for CLI tools
```bash
pipx install poetry
```

Use pyproject.toml + poetry or pip-tools for modern dependency management.

---

## 5. Basics — REPL, Scripts, Running Python

REPL:
```bash
python           # interactive prompt
# or use ipython for a richer REPL
```

Run a script:
```bash
python hello.py
```

Shebang for Unix executables:
```python
#!/usr/bin/env python3
print("Hello")
```
Make executable: `chmod +x script.py` and run `./script.py`.

---

## 6. Data Types & Variables

Primitive types:
- int, float, complex
- bool (subclass of int)
- NoneType (`None`)

Examples:
```python
a = 10           # int
pi = 3.14159     # float
flag = True      # bool
x = None         # NoneType
```

Dynamic typing and assignment:
```python
x = 5
x = "now a string"  # allowed; type changes at runtime
```

Identity vs equality:
```python
a = [1, 2]
b = a
c = [1, 2]
a == c      # True (equality of contents)
a is c      # False (different objects)
a is b      # True  (same object)
```

Type hints (optional):
```python
def add(x: int, y: int) -> int:
    return x + y
```

---

## 7. Common Built-in Collections

Lists
```python
lst = [1, 2, 3]
lst.append(4)
lst[1] = 20
# slicing
sub = lst[1:3]   # [20, 3]
```

Tuples (immutable)
```python
t = (1, 2, 3)
x, y, z = t  # unpacking
```

Sets (unique, unordered)
```python
s = {1, 2, 3}
s.add(4)
s2 = set([2,3,4])
intersection = s & s2
```

Dictionaries (mapping)
```python
d = {'name': 'Alice', 'age': 30}
d['city'] = 'NYC'
for k, v in d.items():
    print(k, v)
```

Comprehensions
```python
squares = [x*x for x in range(10) if x % 2 == 0]
unique_chars = {ch for ch in "abracadabra"}
mapping = {x: x*x for x in range(5)}
```

Common operations and idioms:
- `list(map(func, iterable))`, `filter`, generator expressions to save memory:
```python
gen = (x*x for x in range(1000000))
next(gen)
```

collections module:
```python
from collections import defaultdict, Counter, deque, namedtuple

cnt = Counter(["a","b","a"])
dd = defaultdict(list)
dd['k'].append(1)
```

---

## 8. Operators

Arithmetic: `+ - * / // % **`
Comparison: `== != < <= > >=`
Logical: `and, or, not`
Membership: `in, not in`
Identity: `is, is not`
Bitwise: `& | ^ ~ << >>`
Augmented assignment: `+=, -=, *=, //=, %=`

Examples:
```python
a = 7
b = 3
q, r = divmod(a, b)  # q=2, r=1
```

Chaining comparisons:
```python
0 < x <= 10
```

Boolean short-circuit:
```python
result = value or default  # returns first truthy
```

---

## 9. Strings — operations & formatting

Strings are sequences (immutable). Slicing and methods:
```python
s = "Hello, World!"
s[0:5]                    # 'Hello'
s.lower()                 # 'hello, world!'
s.replace("World", "Py")  # 'Hello, Py!'
```

f-strings (Python 3.6+)
```python
name = "Alice"
age = 30
f"{name} is {age} years old"           # formatted
f"{3.14159:.2f}"                       # '3.14' numeric formatting
```

Bytes and encoding:
```python
b = "café".encode("utf-8")
text = b.decode("utf-8")
```

Multi-line strings and raw strings:
```python
s = """Line1
Line2"""
raw = r"C:\path\to\file"
```

---

## 10. Control Flow — Conditional Statements & Loops

If/elif/else:
```python
if x < 0:
    print("negative")
elif x == 0:
    print("zero")
else:
    print("positive")
```

For loop:
```python
for i in range(5):
    print(i)

for idx, val in enumerate(['a','b','c'], start=1):
    print(idx, val)
```

While loop:
```python
n = 5
while n > 0:
    n -= 1
    if n == 2:
        continue
    if n == 0:
        break
```

Loop `else` executes when loop not terminated by `break`:
```python
for i in range(3):
    if i == 5:
        break
else:
    print("completed without break")
```

Iterators and generators:
```python
it = iter([1,2,3])
next(it)  # 1

def countdown(n):
    while n:
        yield n
        n -= 1

for x in countdown(3):
    print(x)
```

---

## 11. Functions — definition, arguments, scope & advanced topics

Defining functions:
```python
def greet(name: str) -> str:
    return f"Hello, {name}"
```

Argument types:
```python
def func(a, b=2, *args, c=3, **kwargs):
    print(a, b, args, c, kwargs)

# call
func(1, 4, 5, 6, c=7, x=10)
```

Positional-only (Python 3.8+):
```python
def pos_only(a, b, /, c, *, d):
    pass
```

Docstrings:
```python
def add(a, b):
    """Return a + b (simple example)."""
    return a + b
```

Scope rules: LEGB (Local, Enclosing, Global, Built-in)
```python
x = "global"
def outer():
    x = "enclosing"
    def inner():
        nonlocal x
        x = "changed"
    inner()
    return x  # "changed"
```

Lambdas:
```python
square = lambda x: x*x
```

Closures and decorators:
```python
def timer(func):
    import time
    def wrapper(*args, **kwargs):
        start = time.time()
        rv = func(*args, **kwargs)
        print("Elapsed:", time.time() - start)
        return rv
    return wrapper

@timer
def work(n):
    import time; time.sleep(n)
```

Generator functions
```python
def fib(n):
    a, b = 0, 1
    for _ in range(n):
        yield a
        a, b = b, a+b
```

`functools` utilities:
```python
from functools import lru_cache, partial

@lru_cache(maxsize=128)
def fib_cached(n):
    if n < 2: return n
    return fib_cached(n-1) + fib_cached(n-2)
```

---

## 12. Modules & Packages — Importing, Creating, and Best Practices

Create a module `mymodule.py`:
```python
# mymodule.py
PI = 3.14159
def area(r): return PI * r * r

if __name__ == "__main__":
    print("Module test", area(2))
```

Importing:
```python
import mymodule
from mymodule import area, PI
from package.submodule import func
```

Packages: folder with `__init__.py` (Python 3.3+ implicit namespace packages allowed)
```
mypkg/
  __init__.py
  utils.py
  core.py
```

Relative imports within package:
```python
from .utils import helper
from ..common import base
```

`sys.path` and module resolution:
```python
import sys
sys.path  # list of search paths
```

Standard library highlights:
- os, sys, pathlib (file paths), json, datetime, logging, subprocess, threading, multiprocessing, concurrent.futures, itertools, collections, re, urllib, http.client

Example: pathlib usage
```python
from pathlib import Path
p = Path('data')
for f in p.glob('*.csv'):
    print(f.name, f.stat().st_size)
```

Best practice:
- Keep modules small and focused.
- Use virtual environments and lock dependencies.
- Publish reusable packages with `setup.cfg`/`pyproject.toml`.

---

## 13. Error Handling & Debugging Techniques

Try/Except/Finally/Else:
```python
try:
    x = int(input("Enter number: "))
except ValueError as e:
    print("Invalid number", e)
else:
    print("You entered", x)
finally:
    print("Always runs")
```

Catching multiple exceptions:
```python
try:
    ...
except (IOError, OSError) as e:
    ...
```

Raising exceptions:
```python
def divide(a, b):
    if b == 0:
        raise ZeroDivisionError("division by zero")
    return a / b
```

Custom exception:
```python
class MyAppError(Exception):
    pass
```

Tracebacks and logging:
```python
import logging, traceback
logging.basicConfig(level=logging.INFO)

try:
    1/0
except Exception:
    logging.exception("Unexpected error")
    # or print(traceback.format_exc())
```

Debugging tools:
- print debugging (quick)
- pdb (interactive):
```bash
python -m pdb script.py
# or
import pdb; pdb.set_trace()
```
- ipdb (enhanced pdb), pudb (full-screen)
- IDE debuggers: VS Code, PyCharm — set breakpoints, step, inspect variables
- logging with different levels instead of prints for production
- pytest with `-k`, `-s`, and `--pdb` for test-time debugging

Static analysis & type checking:
- `mypy` to check type hints
- linters: `flake8`, `pylint`, `ruff`
- formatters: `black`, `isort`

Common exceptions (samples)
- `SyntaxError`, `IndentationError`
- `TypeError`, `ValueError`, `KeyError`, `IndexError`
- `AttributeError`, `NameError`, `ImportError`, `ModuleNotFoundError`
- `ZeroDivisionError`, `FileNotFoundError`, `PermissionError`, `OSError`

---

## 14. Practical Examples & Small Recipes

CSV reading
```python
import csv
from pathlib import Path

p = Path('data.csv')
with p.open(newline='', encoding='utf-8') as f:
    reader = csv.DictReader(f)
    for row in reader:
        print(row['id'], row['name'])
```

Simple HTTP request (requests)
```python
import requests
r = requests.get("https://api.github.com/repos/python/cpython")
r.raise_for_status()
print(r.json()['stargazers_count'])
```

Unique items preserving order
```python
def unique_preserve(seq):
    seen = set()
    out = []
    for x in seq:
        if x not in seen:
            seen.add(x)
            out.append(x)
    return out
```

Flatten nested list
```python
from itertools import chain
nested = [[1,2], [3,4]]
flat = list(chain.from_iterable(nested))
```

Memoization decorator
```python
from functools import wraps

def memoize(f):
    cache = {}
    @wraps(f)
    def wrapper(*args):
        if args in cache:
            return cache[args]
        cache[args] = f(*args)
        return cache[args]
    return wrapper
```

CSV to JSON quick converter
```python
import csv, json, sys
with open(sys.argv[1]) as f:
    data = list(csv.DictReader(f))
print(json.dumps(data, indent=2))
```

---

## 15. Best Practices & Style Guidelines

- Follow PEP 8 for style; use automated formatters like `black`.
- Use meaningful variable and function names.
- Prefer list/dict comprehensions for clarity and performance (when readable).
- Keep functions small and single-responsibility.
- Use virtual environments per project.
- Pin dependencies for reproducible builds (`pip freeze` or `poetry.lock`).
- Write tests (pytest) and integrate in CI.
- Use logging instead of prints for production code.
- Document public APIs with docstrings and consider Sphinx or mkdocs for documentation.

---

## 16. Further Reading & References

- Official Python docs — https://docs.python.org/3/  
- PEP 8 — Style Guide for Python Code — https://peps.python.org/pep-0008/  
- Real Python — https://realpython.com/  
- "Fluent Python" by Luciano Ramalho — deep dive into idiomatic Python  
- "Effective Python" by Brett Slatkin — practical tips and patterns  
- PyPI — Python Package Index — https://pypi.org/  
- pytest documentation — https://docs.pytest.org/

---

Prepared as a practical Python fundamentals reference with runnable examples to help you learn or teach the language, build projects, and prepare for technical interviews. If you'd like, I can:  
- generate a small sample project repository (with venv, requirements, example scripts and tests),  
- produce a printable cheat-sheet for quick syntax lookup, or  
- expand any section into a deeper tutorial (e.g., decorators, asyncio, typing). Which would you like next?