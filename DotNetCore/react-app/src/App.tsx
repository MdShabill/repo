// import ListGroup from "./Components/ListGroup";

//import Alert from "./Components/Alert";

// function App() {
//   let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   const hendleSelectItem = (item: string) => {
//     console.log(item);
//   };
//   return (
//     <div>
//       <ListGroup
//         items={items}
//         heading="Cities"
//         onSelectItem={hendleSelectItem}
//       />
//     </div>
//   );
// }

// export default App;

// function App() {
//   return (
//     <div>
//       <Alert>Hello World...</Alert>
//     </div>
//   );
// }

// This is for Button and Alert show & hide

// import { useState } from "react";
// import Alert from "./Components/Alert";
// import Button from "./Components/Button";

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

import Like from "./Components/Like";

function App() {
  return (
    <div>
      <Like onClick={() => console.log("clicked")} />
    </div>
  );
}

export default App;
