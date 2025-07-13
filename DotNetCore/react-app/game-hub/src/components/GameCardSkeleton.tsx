//------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45916329
// Improving User Experience With Loading Skeletons

import { Card, CardBody, Skeleton, SkeletonText } from "@chakra-ui/react";

const GameCardSkeleton = () => {
  return (
    <Card borderRadius={10} overflow={"hidden"}>
      <Skeleton height="200px" />
      <CardBody>
        <SkeletonText />
      </CardBody>
    </Card>
  );
};

export default GameCardSkeleton;
