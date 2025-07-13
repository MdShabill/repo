//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915913
// Effect Dependencies

import React, { useEffect, useState } from "react";

const ProductList = ({ category }: { category: string }) => {
  const [products, setProducts] = useState<string[]>([]);

  useEffect(() => {
    console.log("Featching Products in", category);
    setProducts(["Clothing", "HouseHold"]);
  }, [category]);

  return <div>ProductList</div>;
};

export default ProductList;
