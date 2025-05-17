//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915811
//building Expense Filter

// import React from "react";

// interface Props {
//   onSelectCategory: (category: string) => void;
// }

// const ExpenseFilter = ({ onSelectCategory }: Props) => {
//   return (
//     <select
//       className="form-select"
//       onChange={(event) => onSelectCategory(event.target.value)}
//     >
//       <option value="">All Categories</option>
//       <option value="Groceries">Groceries</option>
//       <option value="Utilities">Utilities</option>
//       <option value="Enertainment">Enertainment</option>
//     </select>
//   );
// };

//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915809
//Building the Expense form

import React from "react";
import categories from "../categories";

interface Props {
  onSelectCategory: (category: string) => void;
}

const ExpenseFilter = ({ onSelectCategory }: Props) => {
  return (
    <select
      className="form-select"
      onChange={(event) => onSelectCategory(event.target.value)}
    >
      <option value="">All Categories</option>
      {categories.map((category) => (
        <option key={category} value={category}>
          {category}
        </option>
      ))}
    </select>
  );
};

export default ExpenseFilter;
