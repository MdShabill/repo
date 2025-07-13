//------------------------------------------
//Building a Button Components
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915253
// import React from "react";

// interface Props {
//   children: string;
//   color?: "primary" | "Secondary" | "danger";
//   onClick: () => void;
// }

// const Button = ({ children, onClick, color = "primary" }: Props) => {
//   return (
//     <button className={"btn btn-" + color} onClick={onClick}>
//       {children}
//     </button>
//   );
// };

//------------------------------------------
// Showing & Alert
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915243

import styles from "./Button.module.css";

interface Props {
  children: string;
  color?: "primary" | "Secondary" | "danger";
  onClick: () => void;
}
function Button({ children, onClick, color = "primary" }: Props) {
  return (
    // <button className={"btn btn-" + color} onClick={onClick}>
    //   {children}
    // </button>

    <button
      className={[styles.btn, styles["btn-" + color]].join(" ")}
      onClick={onClick}
    >
      {children}
    </button>
  );
}

export default Button;
