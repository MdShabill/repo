//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915736
// Exercise - Building an ExpandableText Component

import React, { useState } from "react";

interface Props {
  children: string;
  maxchars?: number;
}
const ExpandableText = ({ children, maxchars = 100 }: Props) => {
  const [isExpanded, setExpanded] = useState(false);

  if (children.length <= maxchars) return <p>{children}</p>;

  const text = isExpanded ? children : children.substring(0, maxchars);

  return (
    <p>
      {text}...
      <button onClick={() => setExpanded(!isExpanded)}>
        {isExpanded ? "Less" : "More"}
      </button>
    </p>
  );
};

export default ExpandableText;
