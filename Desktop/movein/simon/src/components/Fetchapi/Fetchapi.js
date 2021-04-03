
import React, { Component } from 'react'
import PropTypes from 'prop-types';


class Fetchapi extends React.Component {
   

    constructor() {
        super()
        this.state = {

            temp: '',
  

        };
        this.fetchData = this.fetchData.bind(this);
        
    }
    


    componentWillMount () {
        this.setState.changeDeaths='2000';
        
        this.fetchData()
    }


    fetchData() {
        fetch('http://api.openweathermap.org/data/2.5/weather?q=Halifax&appid=64021bde9cafb24bbcff4d7093709eb9')
        .then(response => {
          if(response.ok) return response.json();
          throw new Error('Request failed.');
        })
        .then(data => {
          this.setState(({temp:parseInt(data['main']['temp'])-273}));
        })
        .catch(error => {
          console.log(error);
        });
 
    }

    render() {
        return (
            <div className="covid19-container">
 
                <p><b>Temperature in Halifax: </b>{this.state.temp} °C</p>
                {/* <button onClick={() => this.fetchData()}> Fetch Data </button> */}
                {

                (this.state.temp) <= 10 ?
          <img classname="image_container" alt="Canada's Flag" width={125} src="https://i.imgur.com/QVuuHhJ.png"  />
          :
          (this.state.temp >= 10) | parseInt(this.state.temp <= 20) ?
          <img classname="image_container" alt="Canada's Flag" width={125} src="https://upload.wikimedia.org/wikipedia/commons/7/73/Cloudy_sky_%2826171935906%29.jpg"  />
          :
          <img classname="image_container" alt="Canada's Flag" width={125} src="http://news.berkeley.edu/wp-content/uploads/2013/08/Heat-wave-1-410.jpg"  />
                }
          
        

                
            </div>

        )
    }
}


/**
 * Topics you might also like:
 *      - JS Promises -> https://www.w3schools.com/js/js_promise.asp
 *      - Components Lifecycle -> https://reactjs.org/docs/react-component.html#the-component-lifecycle
 */


Fetchapi.propTypes = {};

Fetchapi.defaultProps = {};

export default Fetchapi;
