//-------------------------------
//import { useState } from "react";
// Creating a List Component
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915249
// function ListGroup() {
//   return (
//     <>
//       <br></br>
//       <h1>Lists</h1>
//       <ul className="list-group">
//         <li className="list-group-item">An item</li>
//         <li className="list-group-item">A second item</li>
//         <li className="list-group-item">A third item</li>
//         <li className="list-group-item">A fourth item</li>
//         <li className="list-group-item">And a fifth one..</li>
//       </ul>
//     </>
//   );
// }

//import { useCallback, useState } from "react";

//-------------------------------
// Fragments
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915255
// function ListGroup() {
//   return (
//     <>
//       <br></br>
//       <h1>Lists</h1>
//       <ul className="list-group">
//         <li className="list-group-item">An item</li>
//         <li className="list-group-item">A second item</li>
//         <li className="list-group-item">A third item</li>
//         <li className="list-group-item">A fourth item</li>
//         <li className="list-group-item">And a fifth one..</li>
//       </ul>
//     </>
//   );
// }

//----------------------------------------
// Rendering List
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915245

// function ListGroup() {
//   const items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   return (
//     <>
//       <br></br>
//       <h1>City List from Array</h1>
//       <ul className="list-group">
//         {items.map((item) => (
//           <li key={item}> {item}</li>
//         ))}
//       </ul>
//     </>
//   );
// }

//--------------------------------------------
//  Conditional Rendering
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915252

// function ListGroup() {
//   let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   //items = []; //setting array blank to

//   return (
//     <>
//       <br></br>
//       <h1>List</h1>
//       {items.length == 0 && <p>No Item found</p>}
//       <ul className="list-group">
//         {items.map((item) => (
//           <li className="list-group-item" key={item}>
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//-------------------------------------------
// Handlling Events
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915246
// import { MouseEvent } from "react";

// function ListGroup() {
// let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];

// //Event Handler
// const handleClick = (event: MouseEvent) => console.log(event);

//   return (
//     <>
//       <br></br>
//       <h1>List</h1>
//       {items.length == 0 && <p>No Item found</p>}
//       <ul className="list-group">
//         {items.map((item, index) => (
//           <li className="list-group-item" key={item} onClick={handleClick}>
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//-------------------------------------------
// Managing State
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915242

// import { useState } from "react";

// function ListGroup() {
//   let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//   const [selectedIndex, setSelectedIndex] = useState(-1);

//   return (
//     <>
//       <br></br>
//       <h1>List</h1>
//       {items.length == 0 && <p>No Item found</p>}
//       <ul className="list-group">
//         {items.map((item, index) => (
//           <li
//             className={
//               selectedIndex === index
//                 ? "list-group-item active"
//                 : "list-group-item"
//             }
//             key={item}
//             onClick={() => {
//               setSelectedIndex(index);
//             }}
//           >
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//--------------------------------------------
//Passing Data Via Props
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915256

// import { useState } from "react";

// interface Props {
//   items: string[];
//   heading: string;
// }

// function ListGroup({ items, heading }: Props) {
//   const [selectedIndex, setSelectedIndex] = useState(-1);

//   return (
//     <>
//       <br></br>
//       <h1>{heading}</h1>
//       {items.length == 0 && <p>No Item found</p>}
//       <ul className="list-group">
//         {items.map((item, index) => (
//           <li
//             className={
//               selectedIndex === index
//                 ? "list-group-item active"
//                 : "list-group-item"
//             }
//             key={item}
//             onClick={() => {
//               setSelectedIndex(index);
//             }}
//           >
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//--------------------------------------------
//Passing Function Via Props
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915248

import { useState } from "react";

interface Props {
  items: string[];
  heading: string;
  onSelectItem: (item: string) => void;
}

function ListGroup({ items, heading, onSelectItem }: Props) {
  const [selectedIndex, setSelectedIndex] = useState(-1);

  return (
    <>
      <br></br>
      <h1>{heading}</h1>
      {items.length == 0 && <p>No Item found</p>}
      <ul className="list-group">
        {items.map((item, index) => (
          <li
            className={
              selectedIndex === index
                ? "list-group-item active"
                : "list-group-item"
            }
            key={item}
            onClick={() => {
              setSelectedIndex(index);
              onSelectItem(item);
            }}
          >
            {item}
          </li>
        ))}
      </ul>
    </>
  );
}

// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915379
// Styling Components: Import Our CSS file
//import style from "./ListGroup.module.css";

//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915383
//CSS-In-Js
//import styled from "styled-components";

// const List = styled.ul`
//   list-style: none;
//   padding: 0;
// `;

// const ListItems = styled.li<ListItemProps>`
//   padding: 5px 0;
//   background: ${(props) => (props.active ? "blue" : "none")};
// `;

// interface ListItemProps {
//   active: boolean;
// }

// interface props {
//   items: string[];
//   heading: string;
//   onSelectItem: (item: string) => void;
// }

// function ListGroup({ items, heading, onSelectItem }: props) {
//   const [selectedIndex, setSelectedIndex] = useState(0);

//   return (
//     <>
//       <h1>{heading}</h1>
//       {items.length === 0 && <p>No Items Found</p>}
//       <List className={[style.listGroup, style.container].join(" ")}>
//         {items.map((item, index) => (
//           <ListItems
//             // Here this one line code used withCSS-In-Js
//             active={index === selectedIndex}
//             // This code used with Import Our CSS file
//             // className={
//             //   selectedIndex === index
//             //     ? "list-group-item active"
//             //     : "list-group-item"
//             // }

//             key={item}
//             onClick={() => {
//               setSelectedIndex(index);
//               onSelectItem(item);
//             }}
//           >
//             {item}
//           </ListItems>
//         ))}
//       </List>
//     </>
//   );
// }

export default ListGroup;
