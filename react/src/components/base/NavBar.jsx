import React from "react";
import { Navbar, Nav } from "react-bootstrap";

const NavBar = (user) => {
  return (
    <>
      <Navbar bg="light" variant="light">
        <Navbar.Brand href="/">Navbar</Navbar.Brand>
        {user.id > 0 && user.isLoggedIn === true ? (
          <Nav className="mr-auto">
            <Nav.Link href="#home">Home</Nav.Link>
            <Nav.Link href="#features">Features</Nav.Link>
            <Nav.Link href="#pricing">Pricing</Nav.Link>
          </Nav>
        ) : (
          ""
        )}
      </Navbar>
    </>
  );
};

export default NavBar;
