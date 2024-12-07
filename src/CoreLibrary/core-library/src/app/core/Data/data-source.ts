import { IKeyValue } from '../models/Funcs';

export class DataSource {
  getTestApis(): IKeyValue<string, string>[] {
    return [
      {
        id: 'GET',
        value:
          'https://www.rawpixel.com/api/v1/topics?lang=en&page=1&pagesize=100&sort=trending&type=topic%2Ctopic_group',
      },
      {
        id: 'GET',
        value: 'https://jsonplaceholder.typicode.com/photos',
      },
    ];
  }
  getImages() {
    return [
      'https://i.pinimg.com/736x/9e/36/f5/9e36f5bfa3d753b380e9adb600b45b5c.jpg',
      'https://i.pinimg.com/736x/e8/eb/8a/e8eb8ace03104ccb6a33f4892110a66a.jpg',
      'https://i.pinimg.com/736x/4b/1c/d1/4b1cd1d0eb61a8e52443a08f6e008d30.jpg',
      'https://i.pinimg.com/474x/cd/a2/be/cda2be611e4466e5e6182b627b18f851.jpg',
      'https://i.pinimg.com/236x/ef/d3/1a/efd31ac5f95476c4cce63ea716ac511c.jpg',
      'https://i.pinimg.com/474x/3e/75/69/3e7569d750a771d5c5ed6ebd1c25aa19.jpg',
      'https://i.pinimg.com/474x/5d/92/e9/5d92e99a415012c93e65e529742a902c.jpg',
      'https://i.pinimg.com/474x/d4/3b/98/d43b985a3372a50816b6c243f1401358.jpg',
      'https://i.pinimg.com/236x/b3/60/ae/b360aefcded25044c9f367c78aea4b07.jpg',
      'https://i.pinimg.com/474x/ae/23/4d/ae234ddf0826afc435e6c1388d1cc3b8.jpg',
      'https://i.pinimg.com/474x/b8/ac/51/b8ac51e8e5d9de70114f431574907072.jpg',
      'https://i.pinimg.com/236x/04/7d/45/047d45fd503ad35dacc6f0fcb632cf99.jpg',
      'https://i.pinimg.com/474x/8d/fd/06/8dfd06c0d6d7e108ff6ecf939c0ee1b5.jpg',
      'https://i.pinimg.com/474x/c2/ae/fb/c2aefbed78698218736102f618f7dd7d.jpg',
      'https://i.pinimg.com/474x/a0/e3/8c/a0e38ce2caa7b7d311a02068bc8b9752.jpg',
      'https://i.pinimg.com/236x/f3/34/98/f33498f977fa83129ac66bff0552ca5a.jpg',
      'https://i.pinimg.com/474x/d8/04/6a/d8046afc8ea3bf45a4706be0d2050273.jpg',
      'https://i.pinimg.com/474x/a4/cd/04/a4cd04ac2648cf40b3676d805e33e8ed.jpg',
      'https://i.pinimg.com/474x/97/98/a5/9798a5a8f293bbc39ed3be85550ddde8.jpg',
      'https://i.pinimg.com/474x/d2/52/f9/d252f92cfe6d1a8d02226e1dfb29a04a.jpg',
      'https://i.pinimg.com/474x/a4/a9/e8/a4a9e83186d5a5573be6c42631139c30.jpg',
      'https://i.pinimg.com/474x/be/fe/dd/befeddad10461442c0683fe216fe785a.jpg',
      'https://i.pinimg.com/474x/d2/22/11/d22211908f5c427fd009a77413acda66.jpg',
      'https://i.pinimg.com/474x/3c/eb/e6/3cebe6963d5a043f2c430165b87e3c7d.jpg',
      'https://i.pinimg.com/474x/8d/84/99/8d84996232352416b78fb37daf2d1555.jpg',
      'https://i.pinimg.com/474x/d9/7a/e5/d97ae5c6244cba7bd849b3c8efa97a3f.jpg',
      'https://i.pinimg.com/474x/5f/31/0d/5f310d2a44033e16b222b50de77be91d.jpg',
      'https://i.pinimg.com/474x/88/d5/e4/88d5e4e166a2f46a34d806406b8d5e15.jpg',
      'https://i.pinimg.com/474x/b7/d0/25/b7d025d414730c1863e1d87bdf80169b.jpg',
      'https://i.pinimg.com/474x/79/0f/28/790f286a43b349010e94feab08555f14.jpg',
      'https://i.pinimg.com/474x/76/6b/e3/766be3f9933387e3b8086d16341a450a.jpg',
    ];
  }
  getSummaries() {
    return [
      {
        id: 1,
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
        id: 2,
        header: 'Algorithm',
        lines: [
          'step by step process to solve any problem.',
          'like, we can store same type of data in a array.',
        ],
      },
      {
        id: 3,
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
        id: 4,
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
        id: 5,
        header: 'Array',
        lines: [
          'It is way of storing data of same type in contiguous memory locations.',
          `It having fixed sized, <b>Lookup by Index(Accessing)</b> takes O(1) as we know it have index which makes faster to access, <b>Lookup by value(Searching)</b>  takes O(n)`,
          `<b>Inserting</b> takes O(n) if defined array is filled and redefining size and reallocate the items again
          <b>Deleting</b> takes O(n) on worst case item at first then we have to shift items`,
        ],
      },
      {
        id: 6,
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
        id: 7,
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
        id: 8,
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
        id: 9,
        header: 'Dictionary',
        lines: [
          `It is a Abstract DS also called Maps and Associative Arrays, which store data in form of key/value pairs.
          Here, we use keys it may anything string, number etc. instead of numerical index, and key must have to unique.`,
          `<b>Lookup</b>  O(n)  <b>Access</b>  takes O(n)`,
          `<b>Inserting</b> takes O(n) <b>Deleting</b> takes O(n)`,
        ],
      },
      {
        id: 10,
        header: 'HashTable',
        lines: [
          `It store data in associated manner, it stores data in array format where it have unique index value and data value.`,
          `Number by itself, multiple by number of digits - 1 i.e. (number/number)*(num of digits -1)
          <a href="http://www.gpcet.ac.in/wp-content/uploads/2018/02/ds-FINAL-NOTES-59-88.pdf">Refer Link</a>
          <a href="https://youtu.be/YOfXMQnUlZY?t=7729">Refer Link for Closed and Open hashing</a>`,
        ],
      },
      {
        id: 11,
        header: 'Hashing',
        lines: [
          `It is technique to convert range/list of key values into range/list of indexes of an array`,
          `Eg. we have HashTable of size 20 we use modulus 20 to store it, as we know it may result same index then we do <b>Linear Probing</b> which assigns memory next to it.`,
        ],
      },
      {
        id: 12,
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
        id: 3,
        header: 'Heap',
        lines: [
          `It is a special type of tree where all child nodes are compare to their parent Node `,
          `Min Heap : minimum value among all childrens, Max Heap : maximum value among all childrens, `,
          `Applications: we using in sorting algorithm `,
        ],
      },
      {
        id: 14,
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
            id: 15,
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
  getJmesQueries() {
    return [
      'paths.keys(@)',
      `results[*].{media:{src:image_url,type:'Image'},title:title,text:seo_title_formatted, onHoverShowDetails:'true', showDownload:'true'}`
    ]

  }
}
