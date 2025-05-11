import { useState } from "react";

//  Handling Events
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915246

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
//           <li
//             className="list-group-item"
//             key={item}
//             onClick={() => console.log(item)}
//           >
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//-------------------------------------------
// Conditional Formatting
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915245

// let items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
// //items = []; //setting array blank to

// function ListGroup() {
//   return (
//     <>
//       <br></br>
//       <h1>List</h1>
//       {items.length == 0 && <p>No Item found</p>}
//       <ul className="list-group">
//         {items.map((item, index) => (
//           <li
//             className="list-group-item"
//             key={item}
//             onClick={() => console.log(index, item, " -> Clicked")}
//           >
//             {item}
//           </li>
//         ))}
//       </ul>
//     </>
//   );
// }

//-------------------------------------------
//const items = ["New York", "San Francisco", "Tokyo", "London", "Paris"];
//
// function ListGroup() {
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

//-------------------------------
// hard coded list example
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915255
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

// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915379
// Styling Components: Import Our CSS file
import style from "./ListGroup.module.css";

//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915383
//CSS-In-Js
import styled from "styled-components";

const List = styled.ul`
  list-style: none;
  padding: 0;
`;

const ListItems = styled.li<ListItemProps>`
  padding: 5px 0;
  background: ${(props) => (props.active ? "blue" : "none")};
`;

interface ListItemProps {
  active: boolean;
}

interface props {
  items: string[];
  heading: string;
  onSelectItem: (item: string) => void;
}

function ListGroup({ items, heading, onSelectItem }: props) {
  const [selectedIndex, setSelectedIndex] = useState(0);

  return (
    <>
      <h1>{heading}</h1>
      {items.length === 0 && <p>No Items Found</p>}
      <List className={[style.listGroup, style.container].join(" ")}>
        {items.map((item, index) => (
          <ListItems
            // Here this one line code used withCSS-In-Js
            active={index === selectedIndex}
            // This code used with Import Our CSS file
            // className={
            //   selectedIndex === index
            //     ? "list-group-item active"
            //     : "list-group-item"
            // }

            key={item}
            onClick={() => {
              setSelectedIndex(index);
              onSelectItem(item);
            }}
          >
            {item}
          </ListItems>
        ))}
      </List>
    </>
  );
}

export default ListGroup;
