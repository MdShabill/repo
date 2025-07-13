//import React from "react";
//------------------------------------------
// import ListGroup from "./Components/ListGroup";
// function App() {
//   return (
//     <div>
//       <ListGroup />
//     </div>
//   );
// }

//------------------------------------------
// //Passing Data Via Props
// //https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915256

// import ListGroup from "./Components/ListGroup";

// function App() {
// let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   return (
//     <div>
//       <ListGroup items={items} heading="Cities" />
//     </div>
//   );
// }

//------------------------------------------
//Passing Function Via Props
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915248

// import ListGroup from "./Components/ListGroup";

// function App() {
//   let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   const handleSelectItem = (item: string) => {
//     console.log(item);
//   };

//   return (
//     <div>
//       <ListGroup
//         items={items}
//         heading="Cities"
//         onSelectItem={handleSelectItem}
//       />
//     </div>
//   );
// }

//------------------------------------------
//Passing childern
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915251
// import Alert from "./Components/Alert";

// function App() {
//   return (
//     <div>
//       <Alert>Hello World...</Alert>
//     </div>
//   );
// }

//------------------------------------------
//Building a Button Components
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915253
// import Button from "./Components/Button/Button";

// function App() {
//   return (
//     <div>
//       <Button color="primary" onClick={() => console.log("Clicked")}>
//         My Button
//       </Button>
//     </div>
//   );
// }

//------------------------------------------
// Showing & Alert
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915243

// import { useState } from "react";
// import Alert from "./Components/Alert";
// import Button from "./Components/Button/Button";

// function App() {
//   const [alertVisible, setAlertVisiblity] = useState(false);
//   return (
//     <div>
//       {alertVisible && (
//         <Alert onClose={() => setAlertVisiblity(false)}>My Alert</Alert>
//       )}
//       <Button color="primary" onClick={() => setAlertVisiblity(true)}>
//         My Button
//       </Button>
//     </div>
//   );
// }

//------------------------------------------
// import ListGroup from "./Components/ListGroup";

// //https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915384
// //CSS Modules
// import "./App.css";

// function App() {
//   const items = ["New York", "Los Angeles", "San francisco"];

//   return (
//     <div>
//       <ListGroup heading="Miami" items={items} onSelectItem={() => {}} />
//     </div>
//   );
// }

//------------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915386
//Adding Icons
// import { BsFillCalendarFill } from "react-icons/bs";

// function App() {
//   return (
//     <div>
//       <BsFillCalendarFill color="red" size="40" />
//     </div>
//   );
// }

//------------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915382
//Building A Like Components
// import Like from "./Components/Like";

// function App() {
//   return (
//     <div>
//       <Like onClick={() => console.log("clicked")} />
//     </div>
//   );
// }

//------------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915724
//Understanding The State Hook

// import { useState } from "react";

// function App() {
//   const [isVisible, setAlertVisiblity] = useState(false);
//   //const [isApproved, setApproved] = useState(ture);
//   let count = 0;

//   const handleClick = () => {
//     setAlertVisiblity(true);
//     count++;
//     console.log(isVisible);
//   };
//   return <button onClick={handleClick}>Show</button>;
// }

//------------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915725
//Choosing the state stracture

// import { useState } from "react";

// function App() {
//   const [person, setPerson] = useState({
//     firstName: "",
//     latName: "",
//   });
//   const [isLoading, setLoading] = useState(false);

//   return <div></div>;
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915732
//Keeping Components Pure

// import Message from "./Components/Message";

// function App() {
//   return (
//     <div>
//       <Message />
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915735
//Updating Object

// import { useState } from "react";

// function App() {
//   const [drink, setDrink] = useState({
//     title: "Americano",
//     price: 5,
//   });

//   const handleClick = () => {
//     const newDrink = {
//       title: drink.title,
//       price: 6,
//     };
//     setDrink(newDrink);
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915735
//Updating Nested Object

// import { useState } from "react";

// function App() {
//   const [customer, setCustomer] = useState({
//     name: "John",
//     address: {
//       city: "San Francisco",
//       zipCode: 94111,
//     },
//   });

//   const handleClick = () => {
//     setCustomer({
//       ...customer,
//       address: { ...customer.address, zipCode: 94112 },
//     });
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915729
//Updating Array

// import { useState } from "react";

// function App() {
//   const [tags, setTag] = useState(["happy", "cheerful"]);
//   const handleClick = () => {
//     //Add Array object
//     setTag([...tags, "exciting"]);

//     //Remove Arrayobject
//     setTag(tags.filter((tag) => tag !== "happy"));

//     //Update Array object
//     setTag(tags.map((tag) => (tag === "happy" ? "happiness" : tag)));
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915728
//Updating Array of Object

// import { useState } from "react";

// function App() {
//   const [bugs, setBugs] = useState([
//     { id: 1, title: "bugs 1", fixed: false },
//     { id: 2, title: "bugs 2", fixed: false },
//   ]);

//   const handleClick = () => {
//     setBugs(bugs.map((bug) => (bug.id === 1 ? { ...bug, fixed: true } : bug)));
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915730
//Simplifying Updat logic with immer

// import { useState } from "react";
// import produce from "immer";

// function App() {
//   const [bugs, setBugs] = useState([
//     { id: 1, title: "bugs 1", fixed: false },
//     { id: 2, title: "bugs 2", fixed: false },
//   ]);

//   const handleClick = () => {
//     //setBugs(bugs.map((bug) => (bug.id === 1 ? { ...bug, fixed: true } : bug)));
//     setBugs(
//       produce((draft) => {
//         const bug = draft.find((bug) => bug.id === 1);
//         if (bug) bug.fixed = true;
//       })
//     );
//   };

//   return (
//     <div>
//       {bugs.map((bug) => (
//         <p key={bug.id}>
//           {bug.title} {bug.fixed ? "Fixed" : "New"}
//         </p>
//       ))}
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915734
//Shearing State Between components

// import { useState } from "react";
// import NevBar from "./Components/NevBar";
// import Cart from "./Components/Cart";

// function App() {
//   const [cartItems, setcartItems] = useState(["Product1", "Product2"]);

//   return (
//     <div>
//       <NevBar cartItemscount={cartItems.length} />
//       <Cart cartItems={cartItems} onClear={() => setcartItems([])} />
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915726
//Exercise: 1 - Updating state

// import { useState } from "react";

// function App() {
//   const [game, setgame] = useState({
//     id: 1,
//     player: {
//       name: "John",
//     },
//   });

//   const handleClick = () => {
//     setgame({ ...game, player: { ...game.player, name: "Bob" } });
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915726
//Exercise: 2 - Updating state

// import { useState } from "react";

// function App() {
//   const [pizza, setPizza] = useState({
//     name: "Spicy Pepperoni",
//     toppings: ["Mushroom"],
//   });

//   const handleClick = () => {
//     setPizza({ ...pizza, toppings: [...pizza.toppings, "Cheese"] });
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915726
// Exercise: 3 - Updating state

// import { useState } from "react";

// function App() {
//   const [cart, setcart] = useState({
//     discount: 0.1,
//     items: [
//       { id: 1, title: "Product1", quantity: 1 },
//       { id: 2, title: "Product2", quantity: 1 },
//     ],
//   });

//   const handleClick = () => {
//     setcart({
//       ...cart,
//       items: cart.items.map((item) =>
//         item.id === 1 ? { ...item, quantity: item.quantity + 1 } : item
//       ),
//     });
//   };

//   return (
//     <div>
//       <ul>
//         {cart.items.map((item) => (
//           <li key={item.id}>
//             {item.title} - Quantity: {item.quantity}
//           </li>
//         ))}
//       </ul>
//       <button onClick={handleClick}>Click Me</button>
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915736
// Exercise - Building an ExpandableText Component

// import ExpandableText from "./Components/ExpandableText";

// function App() {
//   return (
//     <div>
//       {/* <ExpandableText maxchars={10}> */}
//       <ExpandableText>Hello World...</ExpandableText>
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915807
// Building a Form

// import Form from "./Components/Form";

// function App() {
//   return (
//     <div>
//       <Form />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915812
// Building Expense List

// import { useState } from "react";
// import ExpenseList from "./expense-tracker/components/ExpenseList";

// function App() {
//   const [expenses, setExpenses] = useState([
//     { id: 1, description: "aaa", amount: 10, category: "Utilities" },
//     { id: 2, description: "bbb", amount: 20, category: "Utilities" },
//     { id: 3, description: "ccc", amount: 30, category: "Utilities" },
//     { id: 4, description: "ddd", amount: 40, category: "Utilities" },
//   ]);

//   return (
//     <div>
//       <ExpenseList
//         expenses={expenses}
//         onDelete={(id) => setExpenses(expenses.filter((e) => e.id !== id))}
//       />
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915811
//building Expense Filter

// import { useState } from "react";
// import ExpenseList from "./expense-tracker/components/ExpenseList";
// import ExpenseFilter from "./expense-tracker/components/ExpenseFilter";

// function App() {
//   const [selectCategory, setSelectedcategory] = useState("");

//   const [expenses, setExpenses] = useState([
//     { id: 1, description: "aaa", amount: 10, category: "Utilities" },
//     { id: 2, description: "bbb", amount: 20, category: "Utilities" },
//     { id: 3, description: "ccc", amount: 30, category: "Utilities" },
//     { id: 4, description: "ddd", amount: 40, category: "Utilities" },
//   ]);

//   const visibleExpense = selectCategory
//     ? expenses.filter((e) => e.category === selectCategory)
//     : expenses;

//   return (
//     <div>
//       <div className="mb-3">
//         <ExpenseFilter
//           onSelectCategory={(category) => setSelectedcategory(category)}
//         />
//       </div>
//       <ExpenseList
//         expenses={visibleExpense}
//         onDelete={(id) => setExpenses(expenses.filter((e) => e.id !== id))}
//       />
//     </div>
//   );
// }

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915809
//Building the Expense form

// import { useState } from "react";
// import ExpenseList from "./expense-tracker/components/ExpenseList";
// import ExpenseFilter from "./expense-tracker/components/ExpenseFilter";
// import ExpenseForm from "./expense-tracker/components/ExpenseForm";

// export const categories = ["Groceries", "Utilities", "Enertainment"];

// function App() {
//   const [selectCategory, setSelectedcategory] = useState("");

//   const [expenses, setExpenses] = useState([
//     { id: 1, description: "aaa", amount: 10, category: "Utilities" },
//     { id: 2, description: "bbb", amount: 20, category: "Utilities" },
//     { id: 3, description: "ccc", amount: 30, category: "Utilities" },
//     { id: 4, description: "ddd", amount: 40, category: "Utilities" },
//   ]);

//   const visibleExpense = selectCategory
//     ? expenses.filter((e) => e.category === selectCategory)
//     : expenses;

//   return (
//     <div>
//       <div className="mb-5">
//         <ExpenseForm />
//       </div>
//       <div className="mb-3">
//         <ExpenseFilter
//           onSelectCategory={(category) => setSelectedcategory(category)}
//         />
//       </div>
//       <ExpenseList
//         expenses={visibleExpense}
//         onDelete={(id) => setExpenses(expenses.filter((e) => e.id !== id))}
//       />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915814
// Integrating with react hook form and zod

// import { useState } from "react";
// import ExpenseList from "./expense-tracker/components/ExpenseList";
// import ExpenseFilter from "./expense-tracker/components/ExpenseFilter";
// import ExpenseForm from "./expense-tracker/components/ExpenseForm";
// import categories from "./expense-tracker/categories";

// function App() {
//   const [selectCategory, setSelectedcategory] = useState("");

//   const [expenses, setExpenses] = useState([
//     { id: 1, description: "aaa", amount: 10, category: "Utilities" },
//     { id: 2, description: "bbb", amount: 20, category: "Utilities" },
//     { id: 3, description: "ccc", amount: 30, category: "Utilities" },
//     { id: 4, description: "ddd", amount: 40, category: "Utilities" },
//   ]);

//   const visibleExpense = selectCategory
//     ? expenses.filter((e) => e.category === selectCategory)
//     : expenses;

//   return (
//     <div>
//       <div className="mb-5">
//         <ExpenseForm />
//       </div>
//       <div className="mb-3">
//         <ExpenseFilter
//           onSelectCategory={(category) => setSelectedcategory(category)}
//         />
//       </div>
//       <ExpenseList
//         expenses={visibleExpense}
//         onDelete={(id) => setExpenses(expenses.filter((e) => e.id !== id))}
//       />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45917480
// Adding an expense

// import { useState } from "react";
// import ExpenseList from "./expense-tracker/components/ExpenseList";
// import ExpenseFilter from "./expense-tracker/components/ExpenseFilter";
// import ExpenseForm from "./expense-tracker/components/ExpenseForm";
// import categories from "./expense-tracker/categories";

// function App() {
//   const [selectCategory, setSelectedcategory] = useState("");

//   const [expenses, setExpenses] = useState([
//     { id: 1, description: "aaa", amount: 10, category: "Utilities" },
//     { id: 2, description: "bbb", amount: 20, category: "Utilities" },
//     { id: 3, description: "ccc", amount: 30, category: "Utilities" },
//     { id: 4, description: "ddd", amount: 40, category: "Utilities" },
//   ]);

//   const visibleExpense = selectCategory
//     ? expenses.filter((e) => e.category === selectCategory)
//     : expenses;

//   return (
//     <div>
//       <div className="mb-5">
//         <ExpenseForm
//           onSubmit={(expense) =>
//             setExpenses([...expenses, { ...expense, id: expenses.length + 1 }])
//           }
//         />
//       </div>
//       <div className="mb-3">
//         <ExpenseFilter
//           onSelectCategory={(category) => setSelectedcategory(category)}
//         />
//       </div>
//       <ExpenseList
//         expenses={visibleExpense}
//         onDelete={(id) => setExpenses(expenses.filter((e) => e.id !== id))}
//       />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915909
// Understanding the effect hook

// import { useEffect, useRef } from "react";

// function App() {
//   const ref = useRef<HTMLInputElement>(null);

//   // After Render
//   useEffect(() => {
//     // Side Effect
//     if (ref.current) ref.current.focus();
//   });

//   useEffect(() => {
//     document.title = "My App";
//   });

//   return (
//     <div>
//       <input ref={ref} type="text" className="form-control" />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915913
// Effect Dependencies

// import { useEffect, useState } from "react";
// import ProductList from "./Components/ProductList";

// function App() {
//   const [category, setCategory] = useState("");

//   return (
//     <div>
//       <select
//         className="form-select"
//         onChange={(event) => setCategory(event.target.value)}
//       >
//         <option value=""></option>
//         <option value="Clothing">Clothing</option>
//         <option value="Household">Household</option>
//       </select>
//       <ProductList category={category} />
//     </div>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915906
// Effect clean up

// import { useEffect } from "react";

// const connect = () => console.log("Connecting");
// const disConnect = () => console.log("Disconnicting");

// function App() {
//   useEffect(() => {
//     connect();

//     return () => disConnect();
//   });

//   return <div></div>;
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915920
// Fetching Data

// import axios from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);

//   useEffect(() => {
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users")
//       .then((res) => setUsers(res.data));
//   }, []);

//   return (
//     <ul>
//       {users.map((user) => (
//         <li key={user.id}>{user.name}</li>
//       ))}
//     </ul>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915915
// Handling Errors

// import axios from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");

//   useEffect(() => {
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/xusers")
//       .then((res) => setUsers(res.data))
//       .catch((err) => setError(err.Message));
//   }, []);

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       <ul>
//         {users.map((user) => (
//           <li key={user.id}>{user.name}</li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915907
// Working With Async and Await

// import axios, { AxiosError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");

//   useEffect(() => {
//     const fetchUsers = async () => {
//       try {
//         const res = await axios.get<User[]>(
//           "https://jsonplaceholder.typicode.com/xusers"
//         );
//         setUsers(res.data);
//       } catch (err) {
//         setError((err as AxiosError).message);
//       }
//     };
//     fetchUsers();
//   }, []);

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       <ul>
//         {users.map((user) => (
//           <li key={user.id}>{user.name}</li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915908
// Cancelling a fetch request

// import axios, { AxiosError, CanceledError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");

//   useEffect(() => {
//     const controller = new AbortController();
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users", {
//         signal: controller.signal,
//       })

//       .then((res) => setUsers(res.data))
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//       });

//     return () => controller.abort();
//   }, []);

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       <ul>
//         {users.map((user) => (
//           <li key={user.id}>{user.name}</li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915910
// Showing a Loading Indicator

// import axios, { AxiosError, CanceledError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     const controller = new AbortController();

//     setLoading(true);
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users", {
//         signal: controller.signal,
//       })

//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => controller.abort();
//   }, []);

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <ul>
//         {users.map((user) => (
//           <li key={user.id}>{user.name}</li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915919
// Deleting Data

// import axios, { AxiosError, CanceledError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     const controller = new AbortController();

//     setLoading(true);
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users", {
//         signal: controller.signal,
//       })

//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => controller.abort();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     axios
//       .delete("https://jsonplaceholder.typicode.com/xusers" + user.id)
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <button
//               className="btn btn-outline-danger"
//               onClick={() => deleteUser(user)}
//             >
//               Delete
//             </button>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915904
// Creating Data

// import axios, { AxiosError, CanceledError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     const controller = new AbortController();

//     setLoading(true);
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users", {
//         signal: controller.signal,
//       })

//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => controller.abort();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     axios
//       .delete("https://jsonplaceholder.typicode.com/users" + user.id)
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const addUser = () => {
//     const originalUsers = [...users];
//     const newUser = { id: 0, name: "Shabill" };
//     setUsers([newUser, ...users]);

//     axios
//       .post("https://jsonplaceholder.typicode.com/users", newUser)
//       .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <button className="btn btn-primary mb-3" onClick={addUser}>
//         Add
//       </button>
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <button
//               className="btn btn-outline-danger"
//               onClick={() => deleteUser(user)}
//             >
//               Delete
//             </button>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915905
// Updating Data

// import axios, { AxiosError, CanceledError } from "axios";
// import { useEffect, useState } from "react";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     const controller = new AbortController();

//     setLoading(true);
//     axios
//       .get<User[]>("https://jsonplaceholder.typicode.com/users", {
//         signal: controller.signal,
//       })

//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => controller.abort();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     axios
//       .delete("https://jsonplaceholder.typicode.com/users" + user.id)
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const addUser = () => {
//     const originalUsers = [...users];
//     const newUser = { id: 0, name: "Shabill" };
//     setUsers([newUser, ...users]);

//     axios
//       .post("https://jsonplaceholder.typicode.com/users", newUser)
//       .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const updateUser = (user: User) => {
//     const originalUsers = [...users];
//     const updatedUser = { ...user, name: user.name + "!" };
//     setUsers(users.map((u) => (u.id === user.id ? updatedUser : u)));

//     axios
//       .patch(
//         "https://jsonplaceholder.typicode.com/users/" + user.id,
//         updatedUser
//       )
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <button className="btn btn-primary mb-3" onClick={addUser}>
//         Add
//       </button>
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <div>
//               <button
//                 className="btn btn-secondery mx-1"
//                 onClick={() => updateUser(user)}
//               >
//                 Update
//               </button>
//               <button
//                 className="btn btn-outline-danger"
//                 onClick={() => deleteUser(user)}
//               >
//                 Delete
//               </button>
//             </div>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915912
// Extracting a Reusable API Clint

// import { useEffect, useState } from "react";
// import apiClient, { CanceledError } from "./services/api-client";

// interface User {
//   id: number;
//   name: string;
// }

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     const controller = new AbortController();

//     setLoading(true);
//     apiClient
//       .get<User[]>("/users", {
//         signal: controller.signal,
//       })

//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => controller.abort();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     apiClient.delete("/users" + user.id).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   const addUser = () => {
//     const originalUsers = [...users];
//     const newUser = { id: 0, name: "Shabill" };
//     setUsers([newUser, ...users]);

//     apiClient
//       .post("/users", newUser)
//       .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const updateUser = (user: User) => {
//     const originalUsers = [...users];
//     const updatedUser = { ...user, name: user.name + "!" };
//     setUsers(users.map((u) => (u.id === user.id ? updatedUser : u)));

//     apiClient.patch("/users/" + user.id, updatedUser).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <button className="btn btn-primary mb-3" onClick={addUser}>
//         Add
//       </button>
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <div>
//               <button
//                 className="btn btn-secondery mx-1"
//                 onClick={() => updateUser(user)}
//               >
//                 Update
//               </button>
//               <button
//                 className="btn btn-outline-danger"
//                 onClick={() => deleteUser(user)}
//               >
//                 Delete
//               </button>
//             </div>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915914
// Extracting the user service

// import { useEffect, useState } from "react";
// import { CanceledError } from "./services/api-client";
// import userService, { User } from "./services/user-service";

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     setLoading(true);
//     const { request, cancel } = userService.getAllUsers();
//     request
//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => cancel();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     userService.deleteUser(user.id).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   const addUser = () => {
//     const originalUsers = [...users];
//     const newUser = { id: 0, name: "Shabill" };
//     setUsers([newUser, ...users]);

//     userService
//       .createUser(newUser)
//       .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const updateUser = (user: User) => {
//     const originalUsers = [...users];
//     const updatedUser = { ...user, name: user.name + "!" };
//     setUsers(users.map((u) => (u.id === user.id ? updatedUser : u)));

//     userService.updateUser(updatedUser).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <button className="btn btn-primary mb-3" onClick={addUser}>
//         Add
//       </button>
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <div>
//               <button
//                 className="btn btn-secondery mx-1"
//                 onClick={() => updateUser(user)}
//               >
//                 Update
//               </button>
//               <button
//                 className="btn btn-outline-danger"
//                 onClick={() => deleteUser(user)}
//               >
//                 Delete
//               </button>
//             </div>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915918
// Creating a Generic HTTP service

// import { useEffect, useState } from "react";
// import { CanceledError } from "./services/api-client";
// import userService, { User } from "./services/user-service";

// function App() {
//   const [users, setUsers] = useState<User[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);

//   useEffect(() => {
//     setLoading(true);
//     const { request, cancel } = userService.getAll<User>();
//     request
//       .then((res) => {
//         setUsers(res.data);
//         setLoading(false);
//       })
//       .catch((err) => {
//         if (err instanceof CanceledError) return;
//         setError(err.Message);
//         setLoading(false);
//       });

//     return () => cancel();
//   }, []);

//   const deleteUser = (user: User) => {
//     const originalUsers = [...users];
//     setUsers(users.filter((u) => u.id !== user.id));

//     userService.delete(user.id).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   const addUser = () => {
//     const originalUsers = [...users];
//     const newUser = { id: 0, name: "Shabill" };
//     setUsers([newUser, ...users]);

//     userService
//       .create(newUser)
//       .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
//       .catch((err) => {
//         setError(err.message);
//         setUsers(originalUsers);
//       });
//   };

//   const updateUser = (user: User) => {
//     const originalUsers = [...users];
//     const updatedUser = { ...user, name: user.name + "!" };
//     setUsers(users.map((u) => (u.id === user.id ? updatedUser : u)));

//     userService.update(updatedUser).catch((err) => {
//       setError(err.message);
//       setUsers(originalUsers);
//     });
//   };

//   return (
//     <>
//       {error && <p className="text-danger">{error}</p>}
//       {isLoading && <div className="spinner-border"></div>}
//       <button className="btn btn-primary mb-3" onClick={addUser}>
//         Add
//       </button>
//       <ul className="list-group">
//         {users.map((user) => (
//           <li
//             key={user.id}
//             className="list-group-item d-flex justify-content-between"
//           >
//             {user.name}
//             <div>
//               <button
//                 className="btn btn-secondery mx-1"
//                 onClick={() => updateUser(user)}
//               >
//                 Update
//               </button>
//               <button
//                 className="btn btn-outline-danger"
//                 onClick={() => deleteUser(user)}
//               >
//                 Delete
//               </button>
//             </div>
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915917
// Creating a Custom Data Featching Hook

import useUsers from "./hooks/useUsers";
import userService, { User } from "./services/user-service";

function App() {
  const { users, error, isLoading, setUsers, setError } = useUsers();

  const deleteUser = (user: User) => {
    const originalUsers = [...users];
    setUsers(users.filter((u: User) => u.id !== user.id));

    userService.delete(user.id).catch((err) => {
      setError(err.message);
      setUsers(originalUsers);
    });
  };

  const addUser = () => {
    const originalUsers = [...users];
    const newUser = { id: 0, name: "Shabill" };
    setUsers([newUser, ...users]);

    userService
      .create(newUser)
      .then(({ data: savedUser }) => setUsers([savedUser, ...users]))
      .catch((err) => {
        setError(err.message);
        setUsers(originalUsers);
      });
  };

  const updateUser = (user: User) => {
    const originalUsers = [...users];
    const updatedUser = { ...user, name: user.name + "!" };
    setUsers(users.map((u: User) => (u.id === user.id ? updatedUser : u)));

    userService.update(updatedUser).catch((err) => {
      setError(err.message);
      setUsers(originalUsers);
    });
  };

  return (
    <>
      {error && <p className="text-danger">{error}</p>}
      {isLoading && <div className="spinner-border"></div>}
      <button className="btn btn-primary mb-3" onClick={addUser}>
        Add
      </button>
      <ul className="list-group">
        {users.map((user: User) => (
          <li
            key={user.id}
            className="list-group-item d-flex justify-content-between"
          >
            {user.name}
            <div>
              <button
                className="btn btn-secondery mx-1"
                onClick={() => updateUser(user)}
              >
                Update
              </button>
              <button
                className="btn btn-outline-danger"
                onClick={() => deleteUser(user)}
              >
                Delete
              </button>
            </div>
          </li>
        ))}
      </ul>
    </>
  );
}

export default App;
