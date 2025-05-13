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

import ExpandableText from "./Components/ExpandableText";

function App() {
  return (
    <div>
      {/* <ExpandableText maxchars={10}> */}
      <ExpandableText>Hello World...</ExpandableText>
    </div>
  );
}

export default App;
