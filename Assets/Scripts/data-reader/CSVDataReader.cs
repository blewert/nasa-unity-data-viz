using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public abstract class CSVDataReader : MonoBehaviour
{
    [System.Serializable]
    public class DataReaderInternalOptions
    {
        /// <summary>
        /// The amount of lines to distribute computation over
        /// </summary>
        public uint linesPerYieldFrame = 100;

        /// <summary>
        /// Skip first line?
        /// </summary>
        public bool skipFirstLine = true;

        /// <summary>
        /// Should we read on awake?
        /// </summary>
        public bool readOnAwake = true;
    }

    [System.Serializable]
    public class DataReaderOptions
    {
        /// <summary>
        /// The path to read the CSV from
        /// </summary>
        public string pathToRead;
    }

    [Header("[Main options]"), SerializeField]
    public DataReaderOptions readerOptions;

    /// <summary>
    /// The reader internal options
    /// </summary>
    [Header("[Reader options]"), SerializeField]
    public DataReaderInternalOptions readerInternalOptions;

    /// <summary>
    /// The actual reader
    /// </summary>
    protected StreamReader reader;

    public virtual void Start()
    {
        //Dont need to read on awake? return
        if(!readerInternalOptions.readOnAwake)
            return;

        //Otherwise, open the file
        bool readStatus = this.Open(readerOptions.pathToRead);

        //Start read daemon
        StartCoroutine(this.StartReadDaemon());
    }

    /// <summary>
    /// Starts the read daemon.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartReadDaemon()
    {
        //Reader is null? error!
        if(this.reader == null)
            throw new UnityException("Reader was null when read daemon was started.");

        //Can it read?
        // if(this.reader.BaseStream == null || !this.reader.BaseStream.CanRead)
        //     throw new UnityException("Can't read underlying base stream.");

        //Temporary line
        string line = default(string);

        //Temporary line/frame counter
        uint lineCount = default(uint);
        uint frameCount = default(uint);

        //Still running?
        bool running = true;

        //Read first line if needed
        if(readerInternalOptions.skipFirstLine)
            this.reader.ReadLine();

        while(running)
        {
            while (lineCount++ < readerInternalOptions.linesPerYieldFrame)
            {
                //Read the line
                line = this.reader.ReadLine();

                //Set running to false, break from this loop
                if(line == null)
                {
                    running = false;
                    break;
                }

                //Otherwise, parse the line
                this.ParseLine(in line);
            }

            //We've read this batch of lines. Wait a frame.
            lineCount = 0;

            // Debug.Log($">> Halted reading, waiting for a frame ({++frameCount} total)");

            //End of frame!
            yield return new WaitForEndOfFrame();
        }

        //Call close, we've read the file
        this.Close();

        //Return null
        yield break;
    }
    

    /// <summary>
    /// Parses a line, called from the read daemon
    /// </summary>
    /// <param name="line"></param>
    public virtual void ParseLine(in string line)
    {
        //Split the line
        var splitLine = line.Split(',');

        //Parse 9 (q) and 10 (per)
        float q = float.Parse(splitLine[9]);
        float per = float.Parse(splitLine[10]);

        // //Do something with it
        // GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // //Set name
        // obj.name = splitLine[2];

        // //Set position
        // obj.transform.position = Vector3.up * q;
    }


    /// <summary>
    /// Opens a CSV from a given path
    /// </summary>
    /// <param name="path"></param>
    public virtual bool Open(string path)
    {
        try
        {
            //Open it up
            this.reader = new StreamReader(path);

            //Return true!
            return true;
        }
        catch(System.Exception e)
        {
            //Show the error
            Debug.LogError("There was an error whilst opening the CSV file:");
            Debug.LogError(e.Message);

            //Close it
            this.Close();
        }

        //Return false to signal error
        return false;
    }


    /// <summary>
    /// Closes a CSV from a given path
    /// </summary>
    /// <returns></returns>
    public virtual void Close()
    {
        // Debug.Log("Closing");
        
        //Close it
        reader.Close();
    }
}
