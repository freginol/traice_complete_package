import React from 'react';
import PropTypes from 'prop-types';
import { Component } from 'react';


class Aboutme extends Component {
  render() {
      return (
      <div className="div-container">
      <h1>Hi there, I'm Fabian</h1>
      <img classname="image_container" alt="Canada's Flag" width={125} src="https://i.imgur.com/D2txsB3.jpg"  />
          <div className="province-container">
          I'm a data analyst with a passion for turning data into actionable insights and meaningful stories. From data extraction to actionable insights, I enjoy the journey with data.

          </div>
          <div className="province-container_2">
          I joined MCDA because I wanted to learn how to analyze, visualize and bring insights out of data.
  
          </div>
                  
          </div>

      )
  }
}   

/**
* Topics you might also like:
*      - Array.map() -> https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/map
*/


Aboutme.propTypes = {};

Aboutme.defaultProps = {};

export default Aboutme;