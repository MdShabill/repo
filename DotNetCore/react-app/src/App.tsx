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

import { useState } from "react";
import Alert from "./Components/Alert";
import Button from "./Components/Button/Button";

function App() {
  const [alertVisible, setAlertVisiblity] = useState(false);
  return (
    <div>
      {alertVisible && (
        <Alert onClose={() => setAlertVisiblity(false)}>My Alert</Alert>
      )}
      <Button color="primary" onClick={() => setAlertVisiblity(true)}>
        My Button
      </Button>
    </div>
  );
}

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

export default App;
