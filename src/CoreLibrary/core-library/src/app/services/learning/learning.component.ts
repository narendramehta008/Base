import { Component, OnInit } from '@angular/core';
import { Summary } from '@app/core/models/Funcs';
import { UtilsService } from '@app/shared/services/utils.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-learning',
  templateUrl: './learning.component.html',
  styleUrl: './learning.component.scss',
})
export class LearningComponent implements OnInit {
  constructor(private utils: UtilsService) {

  }
  expanded = false;
  summarys: Summary[] = [];

  ngOnInit(): void {


    this.utils.getRequest(environment.apiEndPoint.data.get, {
      type: 'Summary'
    }).subscribe({
      next: (response: Summary[]) => {
        console.log(response);
      }, error: (error) => {
        console.log(error);
      }
    });

    this.summarys = [
      {
        header: 'Data Structures',
        lines: [
          'It is the systematic way to organized data so, that we can used it in efficiently.',
          'like, we can store same type of data in a array, stack for redo, undo.',
          `<ol>
          <li>
              <span> <b>Linear DS: </b> When elements are arranged in sequential order.</span>
          </li>
          <li>
              <span> <b>Non-Linear DS: </b> When elements are not arranged in sequential order. like tree have two successor,
              </span>
          </li>
      </ol>
          `,
          `<b>NOTE:</b> Array is static DS since we define fixed size of it.`,
        ],
      },
      {
        header: 'Algorithm',
        lines: [
          'step by step process to solve any problem.',
          'like, we can store same type of data in a array.',
        ],
      },
      {
        header: 'Data types',
        lines: [
          'types of variable we can define on basis of operations we want to perform',
          `<b>Predefined</b> like byte(8), short(16), int(32), long(64), string, decimal,datetime, float and so on`,
          `<b>Dynamic Data Types (.NET)</b> avoid compile time.`,
          `<b>User Defined Data Types</b> defined by user like struct of weekdays.`,
          `<b>Abstract Data Types</b> It is kind of black box (i.e. interface) which hides inner structure and design from user. For eg. user know Push, pop in stack other than nothing like how implemented and working`,
        ],
      },
      {
        header: 'Big O',
        lines: [
          'It describes limiting behaviour of function, when it moving towards particular value or infinity.',
          'A quantifier which measure how efficient is certain DS at different tasks.',
          'like, we can store same type of data in a array.',
          `<ol>We measure efficieny using 4 matrices
          <li> <span> <b>Accessing</b> Lookup by index.</span>  </li>
          <li> <span> <b>Searching </b> Lookup by value.</span>  </li>
          <li> <span> <b>Deleting </b></span>  </li>
          <li> <span> <b>Inserting </b></span>  </li>
      </ol>
          `,
        ],
      },

      {
        header: 'Array',
        lines: [
          'It is way of storing data of same type in contiguous memory locations.',
          `It having fixed sized, <b>Lookup by Index(Accessing)</b> takes O(1) as we know it have index which makes faster to access, <b>Lookup by value(Searching)</b>  takes O(n)`,
          `<b>Inserting</b> takes O(n) if defined array is filled and redefining size and reallocate the items again
          <b>Deleting</b> takes O(n) on worst case item at first then we have to shift items`,
        ],
      },
      {
        header: 'Stack',
        lines: [
          'It is a sequential DS in which we add or remove elements to LIFO principle, It have push, pop, peek, contains methods.',
          `<b>Lookup</b>  O(n)  <b>Access</b>  takes O(n)`,
          `<b>Inserting</b> takes O(1) <b>Deleting</b> takes O(1)`,
        ],
        codes: [
          `public class CustomStack<T>
            {
                private readonly T[] items;
                private int top = -1;
                private readonly int maxSize;
                public CustomStack(int size)
                {
                    items = new T[size];
                    maxSize = size;
                }
        
                public void Push(T item)
                {
                    if (top == maxSize - 1) throw new ArgumentOutOfRangeException();
                    items[++top] = item;
                }
                public T Pop()
                {
                    if (top == -1) throw new InvalidOperationException("Stack empty");
                    return items[top--];
                }
                public T Peek()
                {
                    if (top == -1) throw new InvalidOperationException("Stack empty");
                    return items[top];
                }
        
            }
          `,
        ],
      },
      {
        header: 'Queue',
        lines: [
          'It is a sequential DS which follows FIFO principle, It have head where we enqueue (enter) and tail where we dequeue(delete), peek, contains methods. .',
          `<b>Lookup</b>  O(n)  <b>Access</b>  takes O(n)`,
          `<b>Inserting</b> takes O(1) <b>Deleting</b> takes O(1)`,
        ],
        codes: [
          `public class CustomQueue<T>
            {
                private readonly T[] items;
                private int front = 0;
                private int rear = -1;
                private int itemCount = 0;
                private readonly int maxSize;
                public CustomQueue(int size)
                {
                    items = new T[size];
                    maxSize = size;
                }
                public T Peek()
                {
                    return items[rear];
                }
                public void Enqueue(T item)
                {
                    if (itemCount == maxSize) throw new ArgumentOutOfRangeException();
                    if (rear == maxSize - 1) rear = -1;
                    items[++rear] = item;
                    ++itemCount;
                }
                public T Dequeue()
                {
                    if (rear == -1) throw new InvalidOperationException("Queue Empty.");
                    if (front == maxSize - 1) front = 0;
                    --itemCount;
                    return items[front++];
                }
          }`,
        ],
      },
      {
        header: 'Linked List',
        lines: [
          'List of objects, it have two piece of data one value and other holds address of next object therefore it referred as Linked List',
          `<b>Accessing </b>  takes O(n) it have no index since they are not stroring in sequential`,
          `<b>Inserting at end </b> takes O(1) create a node and points last item to this new item, 
          <b>Inserting at beginning </b> takes O(1) create a node and head points this new item and its address points to previous first item,
          <b>Inserting at middle </b> takes O(n) since here we have to traverse it`,
          `<b>Deleting at start </b> takes O(1) <b>Deleting at end, middle </b> takes O(n) we have to traverse and change address while garbage collector`,
        ],
        codes: [
          `public class CustomLinkedList<T>
            {
        
                private class Node
                {
                    public T Value;
                    public Node NextNode;
                    public Node(T value)
                    {
                        Value = value;
                    }
                }
        
                private Node First;
                private Node Last;
        
                public void AddLast(T value)
                {
                    var node = new Node(value);
                    if (First == null)
                    {
                        Last = First = node;
                    }
                    else
                    {
                        Last.NextNode = node;
                        Last = node;
                    }
        
                }
                public void AddFirst(T value)
                {
                    var node = new Node(value);
                    if (First == null)
                    {
                        Last = First = node;
                    }
                    else
                    {
                        node.NextNode = First;
                        First = node;
                    }
        
                }
        
                public int IndexOf(T value)
                {
                    int index = 0;
                    var current = First;
                    while (current.NextNode != null)
                    {
                        if (current.Value.Equals(value))
                            return index;
                        current = current.NextNode;
                        ++index;
                    }
                    return -1;
                }
        
                public bool Contains(T value)
                {
                    return IndexOf(value) != -1;
                }
        
                public void RemoveLast()
                {
                    if (First == Last)
                    {
                        First = Last = null;
                        return;
                    }
        
                    var current = First;
                    while (current.NextNode != null)
                    {
                        if (current.NextNode == Last)
                        {
                            current.NextNode = null;
                            Last = current;
                            break;
                        }
                    }
                }
                public void RemoveFirst()
                {
                    First = First.NextNode;
                }
              }`,
        ],
      },
      {
        header: 'Dictionary',
        lines: [
          `It is a Abstract DS also called Maps and Associative Arrays, which store data in form of key/value pairs.
          Here, we use keys it may anything string, number etc. instead of numerical index, and key must have to unique.`,
          `<b>Lookup</b>  O(n)  <b>Access</b>  takes O(n)`,
          `<b>Inserting</b> takes O(n) <b>Deleting</b> takes O(n)`,
        ],
      },
      {
        header: 'HashTable',
        lines: [
          `It store data in associated manner, it stores data in array format where it have unique index value and data value.`,
          `Number by itself, multiple by number of digits - 1 i.e. (number/number)*(num of digits -1)
          <a href="http://www.gpcet.ac.in/wp-content/uploads/2018/02/ds-FINAL-NOTES-59-88.pdf">Refer Link</a>
          <a href="https://youtu.be/YOfXMQnUlZY?t=7729">Refer Link for Closed and Open hashing</a>`,
        ],
      },
      {
        header: 'Hashing',
        lines: [
          `It is technique to convert range/list of key values into range/list of indexes of an array`,
          `Eg. we have HashTable of size 20 we use modulus 20 to store it, as we know it may result same index then we do <b>Linear Probing</b> which assigns memory next to it.`,
        ],
      },
      {
        header: 'Trees',
        lines: [
          `An ADS which contains series of linked nodes connected to form a hierachical representation. here Linked list pointing towards multiple locations`,
          `<ol>
          <li> <span><b>Vertice</b> A certain Node in a Tree</span> </li>
          <li> <span><b>Edge</b> A connection between Nodes</span> </li>
          <li> <span><b>Root Node</b> Topmost Node of a Tree</span> </li>
          <li> <span><b>Child Node</b> A certain Node which has an edge connecting it to another Node one level above itself</span> </li>
          <li> <span><b>Parent Node</b> Any Node which has 1 or more child Nodes </span> </li>
          <li> <span><b>Height</b> Number of edges on the longest possible path down towards leaf. </span> </li>
          <li> <span><b>Depth</b> Number of edges required to get from particular node to root Node. </span> </li>
         </ol> `,
          `Eg. file Structure`,
        ],
      },
      {
        header: 'Heap',
        lines: [
          `It is a special type of tree where all child nodes are compare to their parent Node `,
          `Min Heap : minimum value among all childrens, Max Heap : maximum value among all childrens, `,
          `Applications: we using in sorting algorithm `,
        ],
      },
      {
        header: 'Graph',
        lines: [
          `A non linear DS consist of Nodes and Edges`,
          `<ol>
          <li> <span><b>Vertice</b> Finite set of nodes</span> </li>
          <li> <span><b>Edge</b> A connection between Nodes</span> </li>
          <li> <span><b>Edge Set</b> Describes relationship between nodes</span> </li>
          <li> <span><b>UnDirected</b> Here transverse direction is not important similarly vices versa is <b>Directed</b> </span> </li>
         </ol>`,
        ],
        summaryHeader: 'Applications',
        summaries: [
          {
            header: 'Tries',
            lines: [
              `It is tree like structure whose nodes stores letter of alphabets in form of characters, 
                here we traversing and skips the tree where we don't find matching character`,
            ],
          },
        ],
      },
    ];

  }

}
