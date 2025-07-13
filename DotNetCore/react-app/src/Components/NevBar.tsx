//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915734
//Shearing State Between components

import React from "react";

interface Props {
  cartItemscount: number;
}
const NevBar = ({ cartItemscount }: Props) => {
  return <div>NevBar {cartItemscount}</div>;
};

export default NevBar;
